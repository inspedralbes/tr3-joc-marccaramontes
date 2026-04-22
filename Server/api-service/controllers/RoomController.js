class RoomController {
    constructor(roomService) {
        this.roomService = roomService;
    }

    async createRoom(req, res) {
        try {
            const { playerName } = req.body;
            if (!playerName) return res.status(400).json({ error: 'Player name required' });

            const roomId = await this.roomService.createRoom(playerName);
            
            console.log(`[RoomController] Room created: ${roomId} by ${playerName}`);
            res.json({ roomId });
        } catch (error) {
            console.error('[RoomController] Error creating room:', error.message);
            res.status(500).json({ error: error.message });
        }
    }

    async joinRoom(req, res) {
        try {
            const { roomId, playerName } = req.body;
            if (!roomId || !playerName) return res.status(400).json({ error: 'Missing parameters' });

            await this.roomService.joinRoom(roomId, playerName);

            console.log(`[RoomController] Player ${playerName} joined room ${roomId}`);
            res.json({ success: true, roomId });
        } catch (error) {
            console.error('[RoomController] Error joining room:', error.message);
            // Handle specific business errors (full room, non-existent room) with appropriate status codes
            const statusCode = error.message.includes('not exist') ? 404 : 400;
            res.status(statusCode).json({ error: error.message });
        }
    }
}

module.exports = RoomController;
