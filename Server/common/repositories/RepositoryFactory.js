const sqlite3 = require('sqlite3');
const fs = require('fs');
const path = require('path');
const InMemoryUserRepository = require('./implementations/InMemoryUserRepository');
const InMemoryResultRepository = require('./implementations/InMemoryResultRepository');
const SqliteUserRepository = require('./implementations/SqliteUserRepository');
const SqliteResultRepository = require('./implementations/SqliteResultRepository');

/**
 * Factory to manage repository instantiation with fallback logic.
 */
class RepositoryFactory {
    /**
     * Creates repositories based on environment configuration.
     * @returns {Promise<{userRepo: IUserRepository, resultRepo: IResultRepository}>}
     */
    static async createRepositories() {
        // Use DATABASE_URL or DATABASE_FILE, or default to a local sqlite file
        const dbPath = process.env.DATABASE_FILE || process.env.DATABASE_URL || 'database.sqlite';
        
        // If it looks like a postgres URL, we might want to warn or handle it, 
        // but since we are migrating to SQLite, we treat it as a file path or use default.
        const actualDbPath = (dbPath.startsWith('postgres://')) ? 'database.sqlite' : dbPath;

        try {
            console.log(`[RepositoryFactory] Using SQLite database at: ${actualDbPath}`);
            
            // Ensure directory exists if it's a nested path
            const dir = path.dirname(actualDbPath);
            if (dir !== '.' && !fs.existsSync(dir)) {
                fs.mkdirSync(dir, { recursive: true });
            }

            const db = new sqlite3.Database(actualDbPath);

            // Initialize schema
            await this._initializeSchema(db);

            return {
                userRepo: new SqliteUserRepository(actualDbPath),
                resultRepo: new SqliteResultRepository(actualDbPath)
            };
        } catch (error) {
            console.warn(`[RepositoryFactory] SQLite initialization failed: ${error.message}`);
            console.warn('[RepositoryFactory] Falling back to In-Memory repositories.');
            return this._createInMemory();
        }
    }

    /**
     * Creates only the User repository (used by Game Service).
     * @returns {Promise<IUserRepository>}
     */
    static async createUserRepository() {
        const repos = await this.createRepositories();
        return repos.userRepo;
    }

    /**
     * Reads and executes the schema.sql file.
     */
    static _initializeSchema(db) {
        return new Promise((resolve, reject) => {
            const schemaPath = path.join(__dirname, '../scripts/schema.sql');
            if (!fs.existsSync(schemaPath)) {
                console.warn(`[RepositoryFactory] Schema file not found at ${schemaPath}. Skipping initialization.`);
                return resolve();
            }

            const schema = fs.readFileSync(schemaPath, 'utf8');
            db.exec(schema, (err) => {
                if (err) {
                    console.error(`[RepositoryFactory] Failed to initialize schema: ${err.message}`);
                    reject(err);
                } else {
                    console.log('[RepositoryFactory] Database schema initialized successfully.');
                    resolve();
                }
            });
        });
    }

    static _createInMemory() {
        return {
            userRepo: new InMemoryUserRepository(),
            resultRepo: new InMemoryResultRepository()
        };
    }
}

module.exports = RepositoryFactory;
