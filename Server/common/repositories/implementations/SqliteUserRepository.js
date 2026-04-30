const IUserRepository = require('../IUserRepository');
const sqlite3 = require('sqlite3');

class SqliteUserRepository extends IUserRepository {
    constructor(dbPath) {
        super();
        this.db = new sqlite3.Database(dbPath);
    }

    /**
     * Helper to run queries with promises.
     */
    async _query(sql, params = []) {
        return new Promise((resolve, reject) => {
            this.db.all(sql, params, (err, rows) => {
                if (err) reject(err);
                else resolve(rows);
            });
        });
    }

    async _get(sql, params = []) {
        return new Promise((resolve, reject) => {
            this.db.get(sql, params, (err, row) => {
                if (err) reject(err);
                else resolve(row);
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

    async findById(id) {
        // SQLite doesn't have an 'id' column in the 'users' table in current schema, 
        // using username as primary key.
        return this.findByUsername(id);
    }

    async findByUsername(username) {
        const row = await this._get('SELECT * FROM users WHERE username = ?', [username]);
        if (row && row.stats) {
            row.stats = JSON.parse(row.stats);
        }
        return row;
    }

    async save(user) {
        const { username, stats } = user;
        const statsStr = JSON.stringify(stats || {});
        
        await this._run(
            'INSERT INTO users (username, stats) VALUES (?, ?) ON CONFLICT(username) DO UPDATE SET stats = excluded.stats',
            [username, statsStr]
        );
        return this.findByUsername(username);
    }

    async updateStats(username, stats) {
        const statsStr = JSON.stringify(stats || {});
        // SQLite doesn't have a direct 'jsonb' merge operator like Postgres (||), 
        // but it has json_patch for simple merges.
        await this._run(
            'UPDATE users SET stats = json_patch(COALESCE(stats, "{}"), ?) WHERE username = ?',
            [statsStr, username]
        );
        return this.findByUsername(username);
    }
}

module.exports = SqliteUserRepository;
