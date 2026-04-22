-- Schema for User Statistics and Game Results (SQLite version)

CREATE TABLE IF NOT EXISTS users (
    username TEXT PRIMARY KEY,
    stats TEXT DEFAULT '{}',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS results (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    room_id TEXT NOT NULL,
    player_name TEXT NOT NULL,
    score INTEGER NOT NULL,
    duration REAL NOT NULL,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Index for faster result lookups by player
CREATE INDEX IF NOT EXISTS idx_results_player_name ON results(player_name);
-- Index for leaderboard lookups
CREATE INDEX IF NOT EXISTS idx_results_score ON results(score DESC);
