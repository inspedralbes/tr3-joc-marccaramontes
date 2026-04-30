const InMemoryResultRepository = require('../repositories/implementations/InMemoryResultRepository');

describe('InMemoryResultRepository', () => {
    let repo;

    beforeEach(() => {
        repo = new InMemoryResultRepository();
    });

    test('should save a result', async () => {
        const result = { playerName: 'testuser', survivalTime: 120.5 };
        const saved = await repo.save(result);

        expect(saved.id).toBeDefined();
        expect(saved.playerName).toBe('testuser');
        expect(saved.timestamp).toBeDefined();
    });

    test('should get results by player', async () => {
        await repo.save({ playerName: 'user1', score: 100 });
        await repo.save({ playerName: 'user2', score: 200 });
        await repo.save({ playerName: 'user1', score: 150 });

        const user1Results = await repo.getResultsByPlayer('user1');
        expect(user1Results).toHaveLength(2);
        expect(user1Results[0].playerName).toBe('user1');
    });

    test('should return top results sorted by score', async () => {
        await repo.save({ playerName: 'p1', score: 10 });
        await repo.save({ playerName: 'p2', score: 50 });
        await repo.save({ playerName: 'p3', score: 30 });

        const top = await repo.getTopResults(2);
        expect(top).toHaveLength(2);
        expect(top[0].playerName).toBe('p2');
        expect(top[1].playerName).toBe('p3');
    });
});
