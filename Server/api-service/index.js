const express = require('express');
const cors = require('cors');
const bcrypt = require('bcrypt');
const InMemoryUserRepository = require('../common/repositories/implementations/InMemoryUserRepository');
const InMemoryResultRepository = require('../common/repositories/implementations/InMemoryResultRepository');

const app = express();
app.use(cors());
app.use(express.json());

// Initialize repositories
const userRepo = new InMemoryUserRepository();
const resultRepo = new InMemoryResultRepository();

const SALT_ROUNDS = 10;

// ... (rooms object)

app.post('/api/rooms/create', async (req, res) => {
    const { playerName } = req.body;
    if (!playerName) return res.status(400).json({ error: 'Player name required' });

    const roomId = Math.random().toString(36).substring(2, 8).toUpperCase();
    rooms[roomId] = {
        players: [{ playerName, isHost: true }]
    };

    // Requirement 4.2: Passwords must be hashed (even if we don't use them for login yet)
    const dummyPassword = 'password123';
    const hashedPassword = await bcrypt.hash(dummyPassword, SALT_ROUNDS);

    let user = await userRepo.findByUsername(playerName);
    if (!user) {
        user = await userRepo.save({ 
            username: playerName, 
            password: hashedPassword, // Stored hashed
            stats: { gamesPlayed: 0 } 
        });
    }

    console.log(`[API Service] Room created: ${roomId} by ${playerName} (User secured)`);
    res.json({ roomId });
});

app.post('/api/rooms/join', async (req, res) => {
    const { roomId, playerName } = req.body;
    if (!roomId || !playerName) return res.status(400).json({ error: 'Missing parameters' });

    const room = rooms[roomId];
    if (!room) return res.status(404).json({ error: 'Room does not exist' });
    if (room.players.length >= 2) return res.status(400).json({ error: 'Room is full' });

    room.players.push({ playerName, isHost: false });

    // Ensure user exists
    let user = await userRepo.findByUsername(playerName);
    if (!user) {
        await userRepo.save({ username: playerName, stats: { gamesPlayed: 0 } });
    }

    console.log(`[API Service] Player ${playerName} joined room ${roomId}`);
    res.json({ success: true, roomId });
});

app.post('/api/results', async (req, res) => {
    const { roomId, playerName, survivalTime } = req.body;
    
    console.log(`[API Service] Saving result for ${playerName} in ${roomId}`);
    
    await resultRepo.save({
        roomId,
        playerName,
        score: survivalTime,
        duration: survivalTime
    });

    // Update user stats
    const user = await userRepo.findByUsername(playerName);
    if (user) {
        await userRepo.updateStats(playerName, { 
            gamesPlayed: (user.stats.gamesPlayed || 0) + 1,
            bestTime: Math.max(user.stats.bestTime || 0, survivalTime)
        });
    }

    res.json({ success: true });
});

const PORT = process.env.PORT || 3001;
app.listen(PORT, () => {
    console.log(`[API Service] running on http://localhost:${PORT}`);
});
