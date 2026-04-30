const bcrypt = require('bcrypt');

class RoomService {
    constructor(userRepository) {
        this.userRepo = userRepository;
        this.rooms = {}; // Transient room state
        this.SALT_ROUNDS = 10;
    }

    async createRoom(playerName) {
        if (!playerName) throw new Error('Player name required');

        const roomId = Math.random().toString(36).substring(2, 8).toUpperCase();
        
        // Initial room state
        this.rooms[roomId] = {
            players: [{ playerName, isHost: true }]
        };

        // Requirement 4.2: Ensure user exists and password hashed
        const dummyPassword = 'password123';
        const hashedPassword = await bcrypt.hash(dummyPassword, this.SALT_ROUNDS);

        let user = await this.userRepo.findByUsername(playerName);
        if (!user) {
            await this.userRepo.save({ 
                username: playerName, 
                password: hashedPassword,
                stats: { gamesPlayed: 0 } 
            });
        }

        return roomId;
    }

    async joinRoom(roomId, playerName) {
        if (!roomId || !playerName) throw new Error('Missing parameters');

        const room = this.rooms[roomId];
        if (!room) throw new Error('Room does not exist');
        if (room.players.length >= 2) throw new Error('Room is full');

        room.players.push({ playerName, isHost: false });

        // Ensure user exists
        let user = await this.userRepo.findByUsername(playerName);
        if (!user) {
            await this.userRepo.save({ username: playerName, stats: { gamesPlayed: 0 } });
        }

        return true;
    }

    getRoom(roomId) {
        return this.rooms[roomId];
    }
}

module.exports = RoomService;
