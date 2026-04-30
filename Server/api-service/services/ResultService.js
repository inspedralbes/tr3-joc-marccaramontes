class ResultService {
    constructor(resultRepository, userRepository) {
        this.resultRepo = resultRepository;
        this.userRepo = userRepository;
    }

    async saveResult(roomId, playerName, survivalTime) {
        if (!roomId || !playerName) throw new Error('Missing parameters');

        console.log(`[ResultService] Saving result for ${playerName} in ${roomId}`);
        
        await this.resultRepo.save({
            roomId,
            playerName,
            score: survivalTime,
            duration: survivalTime
        });

        // Update user stats
        const user = await this.userRepo.findByUsername(playerName);
        if (user) {
            await this.userRepo.updateStats(playerName, { 
                gamesPlayed: (user.stats.gamesPlayed || 0) + 1,
                bestTime: Math.max(user.stats.bestTime || 0, survivalTime)
            });
        }

        return true;
    }
}

module.exports = ResultService;
