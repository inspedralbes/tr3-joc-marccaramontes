const InMemoryUserRepository = require('../repositories/implementations/InMemoryUserRepository');

describe('InMemoryUserRepository', () => {
    let repo;

    beforeEach(() => {
        repo = new InMemoryUserRepository();
    });

    test('should save and find a user by username', async () => {
        const user = { username: 'testuser', stats: { gamesPlayed: 0 } };
        await repo.save(user);

        const found = await repo.findByUsername('testuser');
        expect(found).toBeDefined();
        expect(found.username).toBe('testuser');
        expect(found.id).toBeDefined();
    });

    test('should return undefined if user not found', async () => {
        const found = await repo.findByUsername('nonexistent');
        expect(found).toBeUndefined();
    });

    test('should update user stats', async () => {
        const user = { username: 'testuser', stats: { gamesPlayed: 0 } };
        await repo.save(user);

        await repo.updateStats('testuser', { gamesPlayed: 5 });
        const updated = await repo.findByUsername('testuser');
        
        expect(updated.stats.gamesPlayed).toBe(5);
    });

    test('should find user by id', async () => {
        const user = { username: 'testuser' };
        const saved = await repo.save(user);

        const found = await repo.findById(saved.id);
        expect(found).toBeDefined();
        expect(found.username).toBe('testuser');
    });
});
