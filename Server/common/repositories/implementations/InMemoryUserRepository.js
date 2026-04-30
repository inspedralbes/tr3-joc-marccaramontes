const IUserRepository = require('../IUserRepository');

class InMemoryUserRepository extends IUserRepository {
    constructor() {
        super();
        this.users = new Map(); // username -> userObject
    }

    async findById(id) {
        return Array.from(this.users.values()).find(u => u.id === id);
    }

    async findByUsername(username) {
        return this.users.get(username);
    }

    async save(user) {
        if (!user.id) user.id = Math.random().toString(36).substring(2, 9);
        this.users.set(user.username, user);
        return user;
    }

    async updateStats(username, stats) {
        const user = this.users.get(username);
        if (user) {
            user.stats = { ...user.stats, ...stats };
            this.users.set(username, user);
        }
        return user;
    }
}

module.exports = InMemoryUserRepository;
