class GameService {
    constructor(userRepository) {
        this.userRepo = userRepository;
        // rooms[roomId] = Set of sockets
        this.rooms = new Map();
    }

    async handleJoinRoom(ws, payload) {
        const { roomId, playerName } = payload;
        
        // Requirement 4.1/4.2 Validation: Check if player exists in common repository
        const user = await this.userRepo.findByUsername(playerName);
        if (!user) {
            console.warn(`[GameService] Player ${playerName} not found in repository!`);
        }

        ws.currentRoomId = roomId;
        ws.currentPlayerName = playerName;

        if (!this.rooms.has(roomId)) {
            this.rooms.set(roomId, new Set());
        }
        this.rooms.get(roomId).add(ws);

        console.log(`[GameService] Player ${playerName} joined room ${roomId}`);
        
        ws.send(JSON.stringify({
            type: 'ROOM_JOINED_CONFIRMED',
            payload: { roomId }
        }));

        this.broadcastToRoom(ws, 'PLAYER_JOINED', { playerName, roomId });
    }

    handleDisconnect(ws) {
        const { currentRoomId, currentPlayerName } = ws;
        if (currentRoomId && this.rooms.has(currentRoomId)) {
            this.rooms.get(currentRoomId).delete(ws);
            if (this.rooms.get(currentRoomId).size === 0) {
                this.rooms.delete(currentRoomId);
            } else {
                this.broadcastToRoom(ws, 'PLAYER_LEFT', { playerName: currentPlayerName });
            }
        }
    }

    broadcastToRoom(sender, type, payload, includeSender = false) {
        const roomId = sender.currentRoomId;
        if (!roomId || !this.rooms.has(roomId)) return;

        const message = JSON.stringify({ type, payload });
        this.rooms.get(roomId).forEach((client) => {
            if (client.readyState === 1) { // WebSocket.OPEN
                if (includeSender || client !== sender) {
                    client.send(message);
                }
            }
        });
    }
}

module.exports = GameService;
