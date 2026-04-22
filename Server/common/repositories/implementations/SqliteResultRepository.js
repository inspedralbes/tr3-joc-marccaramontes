const IResultRepository = require('../IResultRepository');
const sqlite3 = require('sqlite3');

class SqliteResultRepository extends IResultRepository {
    constructor(dbPath) {
        super();
        this.db = new sqlite3.Database(dbPath);
    }

    async _get(sql, params = []) {
        return new Promise((resolve, reject) => {
            this.db.get(sql, params, (err, row) => {
                if (err) reject(err);
                else resolve(row);
            });
        });
    }

    async _query(sql, params = []) {
        return new Promise((resolve, reject) => {
            this.db.all(sql, params, (err, rows) => {
                if (err) reject(err);
                else resolve(rows);
            });
        });
    }

    async _run(sql, params = []) {
        return new Promise((resolve, reject) => {
            this.db.run(sql, params, function(err) {
                if (err) reject(err);
                else resolve({ id: this.lastID, changes: this.changes });
            });
        });
    }

    async save(result) {
        const { roomId, playerName, score, duration, timestamp } = result;
        const res = await this._run(
            'INSERT INTO results (room_id, player_name, score, duration, timestamp) VALUES (?, ?, ?, ?, COALESCE(?, CURRENT_TIMESTAMP))',
            [roomId, playerName, score, duration, timestamp]
        );
        return { id: res.id, ...result };
    }

    async getTopResults(limit = 10) {
        return await this._query(
            'SELECT * FROM results ORDER BY score DESC LIMIT ?',
            [limit]
        );
    }

    async getResultsByPlayer(username) {
        return await this._query(
            'SELECT * FROM results WHERE player_name = ?',
            [username]
        );
    }
}

module.exports = SqliteResultRepository;
