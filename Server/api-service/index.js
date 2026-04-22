const express = require('express');
const cors = require('cors');

// Repositories
const InMemoryUserRepository = require('../common/repositories/implementations/InMemoryUserRepository');
const InMemoryResultRepository = require('../common/repositories/implementations/InMemoryResultRepository');

// Services
const RoomService = require('./services/RoomService');
const ResultService = require('./services/ResultService');

// Controllers
const RoomController = require('./controllers/RoomController');
const ResultController = require('./controllers/ResultController');

const app = express();
app.use(cors());
app.use(express.json());

// 1. Dependency Injection - Instantiate Repositories
// In a real scenario, we could switch to PostgresUserRepository based on env vars
const userRepo = new InMemoryUserRepository();
const resultRepo = new InMemoryResultRepository();

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
app.post('/rooms/create', (req, res) => roomController.createRoom(req, res));
app.post('/rooms/join', (req, res) => roomController.joinRoom(req, res));

// Result Routes
app.post('/results', (req, res) => resultController.saveResults(req, res));

const PORT = process.env.PORT || 3001;
app.listen(PORT, () => {
    console.log(`[API Service] layered architecture running on http://localhost:${PORT}`);
});
