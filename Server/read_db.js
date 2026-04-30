const sqlite3 = require('sqlite3').verbose();
const path = require('path');

async function readTable(dbPath, table) {
    return new Promise((resolve, reject) => {
        const db = new sqlite3.Database(dbPath, sqlite3.OPEN_READONLY, (err) => {
            if (err) reject(err);
        });

        db.all(`SELECT * FROM ${table}`, [], (err, rows) => {
            if (err) {
                db.close();
                reject(err);
            } else {
                db.close();
                resolve(rows);
            }
        });
    });
}

async function main() {
    const dbs = [
        'api-service/database.sqlite',
        'game-service/database.sqlite'
    ];

    for (const dbPath of dbs) {
        console.log(`\n--- Database: ${dbPath} ---`);
        try {
            const users = await readTable(dbPath, 'users');
            console.log('Users:');
            console.table(users);
            
            const results = await readTable(dbPath, 'results');
            console.log('Results:');
            console.table(results);
        } catch (err) {
            console.log(`Error reading ${dbPath}: ${err.message}`);
        }
    }
}

main();
