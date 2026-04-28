const RoomService = require('../../api-service/services/RoomService');
const InMemoryUserRepository = require('../repositories/implementations/InMemoryUserRepository');

describe('RoomService with InMemoryRepository', () => {
    let roomService;
    let userRepo;

    beforeEach(() => {
        userRepo = new InMemoryUserRepository();
        roomService = new RoomService(userRepo);
    });

    test('should create a room and save a new user', async () => {
        const roomId = await roomService.createRoom('HostPlayer');
        
        expect(roomId).toBeDefined();
        expect(roomId).toHaveLength(6);
        
        const user = await userRepo.findByUsername('HostPlayer');
        expect(user).toBeDefined();
        expect(user.username).toBe('HostPlayer');
        
        const room = roomService.getRoom(roomId);
        expect(room).toBeDefined();
        expect(room.players[0].playerName).toBe('HostPlayer');
    });

    test('should allow a player to join an existing room', async () => {
        const roomId = await roomService.createRoom('Host');
        const success = await roomService.joinRoom(roomId, 'Guest');
        
        expect(success).toBe(true);
        const room = roomService.getRoom(roomId);
        expect(room.players).toHaveLength(2);
        expect(room.players[1].playerName).toBe('Guest');
    });

    test('should throw error when joining non-existent room', async () => {
        await expect(roomService.joinRoom('NONE', 'Player'))
            .rejects.toThrow('Room does not exist');
    });

    test('should throw error when room is full', async () => {
        const roomId = await roomService.createRoom('P1');
        await roomService.joinRoom(roomId, 'P2');
        
        await expect(roomService.joinRoom(roomId, 'P3'))
            .rejects.toThrow('Room is full');
    });
});
