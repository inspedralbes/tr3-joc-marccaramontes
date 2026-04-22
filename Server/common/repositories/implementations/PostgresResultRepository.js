const IResultRepository = require('../IResultRepository');
const { Pool } = require('pg');

class PostgresResultRepository extends IResultRepository {
    constructor(config) {
        super();
        this.pool = new Pool(config);
    }

    async save(result) {
        const { roomId, playerName, score, duration } = result;
        const res = await this.pool.query(
            'INSERT INTO results (room_id, player_name, score, duration) VALUES ($1, $2, $3, $4) RETURNING *',
            [roomId, playerName, score, duration]
        );
        return res.rows[0];
    }

    async getTopResults(limit = 10) {
        const res = await this.pool.query(
            'SELECT * FROM results ORDER BY score DESC LIMIT $1',
            [limit]
        );
        return res.rows;
    }

    async getResultsByPlayer(username) {
        const res = await this.pool.query(
            'SELECT * FROM results WHERE player_name = $1',
            [username]
        );
        return res.rows;
    }
}

module.exports = PostgresResultRepository;
