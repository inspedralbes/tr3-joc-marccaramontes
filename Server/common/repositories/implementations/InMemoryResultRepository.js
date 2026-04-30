const IResultRepository = require('../IResultRepository');

class InMemoryResultRepository extends IResultRepository {
    constructor() {
        super();
        this.results = [];
    }

    async save(result) {
        result.id = Math.random().toString(36).substring(2, 9);
        result.timestamp = new Date();
        this.results.push(result);
        return result;
    }

    async getTopResults(limit = 10) {
        return [...this.results]
            .sort((a, b) => b.score - a.score)
            .slice(0, limit);
    }

    async getResultsByPlayer(username) {
        return this.results.filter(r => r.playerName === username);
    }
}

module.exports = InMemoryResultRepository;
