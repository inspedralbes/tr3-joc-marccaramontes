class SocketController {
    constructor(gameService) {
        this.gameService = gameService;
    }

    async handleMessage(ws, message) {
        try {
            const packet = JSON.parse(message);
            const { type, payload } = packet;

            switch (type) {
                case 'JOIN_ROOM':
                    await this.gameService.handleJoinRoom(ws, payload);
                    break;
                case 'MOVE':
                case 'SHOOT':
                case 'SPAWN_ENEMY':
                case 'ENEMY_SYNC':
                case 'DEATH':
                    this.gameService.broadcastToRoom(ws, type, payload);
                    break;
                case 'START_MATCH':
                case 'start_match':
                    this.gameService.broadcastToRoom(ws, 'START_MATCH', payload, true);
                    break;
                case 'LEAVE_ROOM':
                    this.gameService.handleDisconnect(ws);
                    break;
                default:
                    console.warn(`[SocketController] Unknown message type: ${type}`);
            }
        } catch (err) {
            console.error('[SocketController] Error parsing message:', err.message);
        }
    }

    handleDisconnect(ws) {
        this.gameService.handleDisconnect(ws);
    }
}

module.exports = SocketController;
