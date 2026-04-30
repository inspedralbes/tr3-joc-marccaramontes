const { WebSocketServer } = require('ws');

// Repositories
const RepositoryFactory = require('../common/repositories/RepositoryFactory');

// Services
const GameService = require('./services/GameService');

// Controllers
const SocketController = require('./controllers/SocketController');

async function startServer() {
    const PORT = process.env.PORT || 3002;
    const wss = new WebSocketServer({ port: PORT });

    // 1. Dependency Injection - Instantiate Repositories via Factory
    const userRepo = await RepositoryFactory.createUserRepository();

    // 2. Dependency Injection - Instantiate Services
    const gameService = new GameService(userRepo);

    // 3. Dependency Injection - Instantiate Controllers
    const socketController = new SocketController(gameService);

    console.log(`[Game Service] WebSocket server running on ws://localhost:${PORT}`);

    wss.on('connection', (ws) => {
        console.log('[Game Service] New socket connection');

        ws.on('message', async (message) => {
            await socketController.handleMessage(ws, message);
        });

        ws.on('close', () => {
            socketController.handleDisconnect(ws);
            console.log('[Game Service] Socket connection closed');
        });

        ws.on('error', (error) => {
            console.error('[Game Service] Socket error:', error.message);
        });
    });
}

startServer().catch(err => {
    console.error('[Game Service] Failed to start:', err);
    process.exit(1);
});
