const { WebSocketServer } = require('ws');
const InMemoryUserRepository = require('../common/repositories/implementations/InMemoryUserRepository');

const PORT = process.env.PORT || 3002;
const wss = new WebSocketServer({ port: PORT });

// Initialize repositories (using InMemory for now)
const userRepo = new InMemoryUserRepository();

// rooms[roomId] = Set of sockets
const rooms = new Map();

console.log(`[Game Service] WebSocket server running on ws://localhost:${PORT}`);

wss.on('connection', (ws) => {
    console.log('[Game Service] New connection');
    let currentRoomId = null;
    let currentPlayerName = null;

    ws.on('message', async (message) => {
        try {
            const packet = JSON.parse(message);
            const { type, payload } = packet;

            switch (type) {
                case 'JOIN_ROOM':
                    await handleJoinRoom(ws, payload);
                    break;
                case 'MOVE':
                case 'SHOOT':
                case 'SPAWN_ENEMY':
                case 'DEATH':
                    broadcastToRoom(ws, type, payload);
                    break;
                case 'START_MATCH':
                    broadcastToRoom(ws, type, payload, true);
                    break;
                default:
                    console.warn(`[Game Service] Unknown message type: ${type}`);
            }
        } catch (err) {
            console.error('[Game Service] Error parsing message:', err.message);
        }
    });

    ws.on('close', () => {
        if (currentRoomId && rooms.has(currentRoomId)) {
            rooms.get(currentRoomId).delete(ws);
            if (rooms.get(currentRoomId).size === 0) {
                rooms.delete(currentRoomId);
            } else {
                broadcastToRoom(ws, 'PLAYER_LEFT', { playerName: currentPlayerName });
            }
        }
    });

    async function handleJoinRoom(socket, payload) {
        const { roomId, playerName } = payload;
        
        // Requirement 4.1/4.2 Validation: Check if player exists in common repository
        const user = await userRepo.findByUsername(playerName);
        if (!user) {
            console.warn(`[Game Service] Player ${playerName} not found in repository!`);
            // We still allow it for now for flexibility, but logging it.
        }

        currentRoomId = roomId;
        currentPlayerName = playerName;

        if (!rooms.has(roomId)) {
            rooms.set(roomId, new Set());
        }
        rooms.get(roomId).add(socket);

        console.log(`[Game Service] Player ${playerName} joined room ${roomId}`);
        
        socket.send(JSON.stringify({
            type: 'ROOM_JOINED_CONFIRMED',
            payload: { roomId }
        }));

        broadcastToRoom(socket, 'PLAYER_JOINED', { playerName, roomId });
    }

    function broadcastToRoom(sender, type, payload, includeSender = false) {
        if (!currentRoomId || !rooms.has(currentRoomId)) return;

        const message = JSON.stringify({ type, payload });
        rooms.get(currentRoomId).forEach((client) => {
            if (client.readyState === 1) {
                if (includeSender || client !== sender) {
                    client.send(message);
                }
            }
        });
    }
});
