const express = require('express');
const cors = require('cors');

// Repositories
const RepositoryFactory = require('../common/repositories/RepositoryFactory');

// Services
const RoomService = require('./services/RoomService');
const ResultService = require('./services/ResultService');

// Controllers
const RoomController = require('./controllers/RoomController');
const ResultController = require('./controllers/ResultController');

const app = express();
app.use(cors());
app.use(express.json());

// Validation Middleware
const validateRequest = (requiredFields) => {
    return (req, res, next) => {
        const missingFields = requiredFields.filter(field => !req.body[field]);
        if (missingFields.length > 0) {
            return res.status(400).json({
                error: 'Bad Request',
                message: `Missing required fields: ${missingFields.join(', ')}`,
                code: 400
            });
        }
        next();
    };
};

async function startServer() {
    // 1. Dependency Injection - Instantiate Repositories via Factory
    const { userRepo, resultRepo } = await RepositoryFactory.createRepositories();

    // 2. Dependency Injection - Instantiate Services
    const roomService = new RoomService(userRepo);
    const resultService = new ResultService(resultRepo, userRepo);

    // 3. Dependency Injection - Instantiate Controllers
    const roomController = new RoomController(roomService);
    const resultController = new ResultController(resultService);

    // --- Routes ---

    // Health Check
    app.get('/health', (req, res) => res.json({ status: 'API Service is healthy' }));

    // Room Routes
    app.post('/rooms/create', validateRequest(['playerName']), (req, res) => roomController.createRoom(req, res));
    app.post('/rooms/join', validateRequest(['roomId', 'playerName']), (req, res) => roomController.joinRoom(req, res));

    // Result Routes
    app.post('/results', validateRequest(['roomId', 'playerName', 'survivalTime']), (req, res) => resultController.saveResults(req, res));

    const PORT = process.env.PORT || 3001;
    app.listen(PORT, () => {
        console.log(`[API Service] layered architecture running on http://localhost:${PORT}`);
    });
}

startServer().catch(err => {
    console.error('[API Service] Failed to start:', err);
    process.exit(1);
});
