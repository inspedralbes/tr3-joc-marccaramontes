const express = require('express');
const http = require('http');
const { Server } = require('socket.io');

const app = express();

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
        </body>
        </html>
    `);
});

app.get('/favicon.ico', (req, res) => res.status(204).end());

const server = http.createServer(app);
const io = new Server(server, {
    cors: {
        origin: "*",
        methods: ["GET", "POST"]
    }
});

const PORT = process.env.PORT || 3000;

// Almacenamiento volátil de salas
// rooms[roomId] = { hostId, players: [ {id, socketId} ] }
const rooms = {};

io.on('connection', (socket) => {
    console.log(`Usuario conectado: ${socket.id}`);

    // --- Gestión de Salas ---

    socket.on('create_room', () => {
        const roomId = Math.random().toString(36).substring(2, 8).toUpperCase();
        rooms[roomId] = {
            hostId: socket.id,
            players: [{ id: socket.id, isHost: true }]
        };
        socket.join(roomId);
        socket.emit('room_created', { roomId, isHost: true });
        console.log(`Sala creada: ${roomId} por ${socket.id}`);
    });

    socket.on('join_room', (roomId) => {
        if (rooms[roomId]) {
            if (rooms[roomId].players.length < 2) {
                rooms[roomId].players.push({ id: socket.id, isHost: false });
                socket.join(roomId);
                socket.emit('room_joined', { roomId, isHost: false });
                
                // Notificar a todos en la sala que la partida puede empezar
                io.to(roomId).emit('player_joined', { playerId: socket.id, totalPlayers: rooms[roomId].players.length });
                console.log(`Usuario ${socket.id} se unió a la sala ${roomId}`);
            } else {
                socket.emit('error_message', 'La sala está llena');
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
        io.to(data.roomId).emit('game_over', {
            playerId: socket.id,
            survivalTime: data.survivalTime
        });
    });

    socket.on('disconnect', () => {
        console.log(`Usuario desconectado: ${socket.id}`);
        // Limpieza básica de salas (opcional para prototipo)
        for (const roomId in rooms) {
            const index = rooms[roomId].players.findIndex(p => p.id === socket.id);
            if (index !== -1) {
                rooms[roomId].players.splice(index, 1);
                io.to(roomId).emit('player_left', { playerId: socket.id });
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
