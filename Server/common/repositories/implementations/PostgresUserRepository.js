const IUserRepository = require('../IUserRepository');
const { Pool } = require('pg');

class PostgresUserRepository extends IUserRepository {
    constructor(config) {
        super();
        this.pool = new Pool(config);
    }

    async findById(id) {
        const res = await this.pool.query('SELECT * FROM users WHERE id = $1', [id]);
        return res.rows[0];
    }

    async findByUsername(username) {
        const res = await this.pool.query('SELECT * FROM users WHERE username = $1', [username]);
        return res.rows[0];
    }

    async save(user) {
        const { username, stats } = user;
        const res = await this.pool.query(
            'INSERT INTO users (username, stats) VALUES ($1, $2) ON CONFLICT (username) DO UPDATE SET stats = $2 RETURNING *',
            [username, JSON.stringify(stats)]
        );
        return res.rows[0];
    }

    async updateStats(username, stats) {
        const res = await this.pool.query(
            'UPDATE users SET stats = stats || $2::jsonb WHERE username = $1 RETURNING *',
            [username, JSON.stringify(stats)]
        );
        return res.rows[0];
    }
}

module.exports = PostgresUserRepository;
