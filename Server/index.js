const express = require('express');
const http = require('http');
const { Server } = require('socket.io');

const app = express();
const cors = require('cors');

// Middleware
app.use(cors());
app.use(express.json());

// Rutas básicas para evitar 404 y el CSP restrictivo por defecto de Express 5
app.get('/', (req, res) => {
    res.send(`
        <!DOCTYPE html>
        <html lang="es">
        <head>
            <meta charset="UTF-8">
            <title>Servidor de Juego</title>
        </head>
        <body>
            <h1>Servidor de juego Socket.io activo</h1>
            <p>Estado: Corriendo correctamente</p>
            <p>Endpoints REST disponibles para el prototipo.</p>
        </body>
        </html>
    `);
});

app.get('/favicon.ico', (req, res) => res.status(204).end());

// --- Endpoints REST (UnityWebRequest) ---

// Almacenamiento volátil de salas
// rooms[roomId] = { hostId, players: [ {id, playerName, socketId, isHost} ] }
const rooms = {};

app.post('/api/rooms/create', (req, res) => {
    const { playerName } = req.body;
    if (!playerName) return res.status(400).json({ error: 'Nombre de jugador requerido' });

    const roomId = Math.random().toString(36).substring(2, 8).toUpperCase();
    rooms[roomId] = {
        hostId: null, // Se asignará cuando el socket se conecte
        players: [{ playerName, socketId: null, isHost: true }]
    };

    console.log(`Sala creada vía REST: ${roomId} por ${playerName}`);
    res.json({ roomId });
});

app.post('/api/rooms/join', (req, res) => {
    const { roomId, playerName } = req.body;
    if (!roomId || !playerName) return res.status(400).json({ error: 'Faltan parámetros' });

    const room = rooms[roomId];
    if (!room) return res.status(404).json({ error: 'La sala no existe' });
    if (room.players.length >= 2) return res.status(400).json({ error: 'La sala está llena' });

    room.players.push({ playerName, socketId: null, isHost: false });
    console.log(`Jugador ${playerName} pre-registrado vía REST en sala ${roomId}`);
    res.json({ success: true, roomId });
});

app.post('/api/results', (req, res) => {
    const { roomId, playerName, survivalTime } = req.body;
    console.log(`[RESULTADOS] Sala: ${roomId}, Jugador: ${playerName}, Tiempo: ${survivalTime}s`);
    // En un proyecto real, aquí guardaríamos en una base de datos.
    res.json({ success: true });
});

const server = http.createServer(app);
const io = new Server(server, {
    cors: {
        origin: "*",
        methods: ["GET", "POST"]
    }
});

const PORT = process.env.PORT || 3000;

io.on('connection', (socket) => {
    console.log(`Socket conectado: ${socket.id}`);

    // --- Vinculación de Socket con Sala (tras el registro REST) ---

    socket.on('join_room_socket', (data) => {
        const { roomId, playerName } = data;
        const room = rooms[roomId];

        if (room) {
            const player = room.players.find(p => p.playerName === playerName);
            if (player) {
                player.socketId = socket.id;
                socket.join(roomId);
                
                if (player.isHost) room.hostId = socket.id;

                socket.emit('room_joined_confirmed', { roomId, isHost: player.isHost });
                
                // Notificar a todos en la sala
                io.to(roomId).emit('player_joined', { 
                    playerId: socket.id, 
                    playerName: player.playerName,
                    totalPlayers: room.players.length 
                });
                
                console.log(`Socket ${socket.id} vinculado a ${playerName} en sala ${roomId}`);
            } else {
                socket.emit('error_message', 'Jugador no registrado en esta sala');
            }
        } else {
            socket.emit('error_message', 'La sala no existe');
        }
    });

    // --- Sincronización de Juego ---

    socket.on('start_match', (roomId) => {
        if (rooms[roomId] && rooms[roomId].hostId === socket.id) {
            io.to(roomId).emit('match_started');
            console.log(`Partida iniciada en sala ${roomId}`);
        }
    });

    socket.on('update_position', (data) => {
        // data: { roomId, x, y, rotation }
        socket.to(data.roomId).emit('player_moved', {
            playerId: socket.id,
            x: data.x,
            y: data.y,
            rotation: data.rotation
        });
    });

    socket.on('spawn_enemy', (data) => {
        // data: { roomId, enemyId, type, x, y }
        socket.to(data.roomId).emit('enemy_spawned', data);
    });

    socket.on('player_shoot', (data) => {
        // data: { roomId, x, y, rotation }
        socket.to(data.roomId).emit('player_shot', {
            playerId: socket.id,
            x: data.x,
            y: data.y,
            rotation: data.rotation
        });
    });

    socket.on('player_death', (data) => {
        // data: { roomId, survivalTime }
        // Se mantiene el evento para notificar el fin inmediato, 
        // pero el resultado oficial se enviará por REST.
        io.to(data.roomId).emit('game_over', {
            playerId: socket.id,
            survivalTime: data.survivalTime
        });
    });

    socket.on('disconnect', () => {
        console.log(`Socket desconectado: ${socket.id}`);
        for (const roomId in rooms) {
            const playerIndex = rooms[roomId].players.findIndex(p => p.socketId === socket.id);
            if (playerIndex !== -1) {
                const playerName = rooms[roomId].players[playerIndex].playerName;
                rooms[roomId].players.splice(playerIndex, 1);
                io.to(roomId).emit('player_left', { playerId: socket.id, playerName });
                
                if (rooms[roomId].players.length === 0) {
                    delete rooms[roomId];
                }
                break;
            }
        }
    });
});

server.listen(PORT, () => {
    console.log(`Servidor de juego corriendo en http://localhost:${PORT}`);
});
