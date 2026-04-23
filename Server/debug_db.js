const sqlite3 = require('sqlite3').verbose();

async function checkData(dbPath) {
    return new Promise((resolve) => {
        const db = new sqlite3.Database(dbPath, sqlite3.OPEN_READONLY, (err) => {
            if (err) {
                console.log(`Could not open ${dbPath}: ${err.message}`);
                return resolve();
            }
        });

        db.all("SELECT * FROM users", [], (err, rows) => {
            console.log(`\n--- ${dbPath} ---`);
            if (err) {
                console.log(`Error reading users: ${err.message}`);
            } else {
                console.log(`Users (${rows.length}):`, rows);
            }

            db.all("SELECT * FROM results", [], (err, rows) => {
                if (err) {
                    console.log(`Error reading results: ${err.message}`);
                } else {
                    console.log(`Results (${rows.length}):`, rows);
                }
                db.close();
                resolve();
            });
        });
    });
}

async function main() {
    await checkData('api-service/database.sqlite');
    await checkData('game-service/database.sqlite');
}

main();
