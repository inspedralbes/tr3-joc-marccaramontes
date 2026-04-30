class ResultController {
    constructor(resultService) {
        this.resultService = resultService;
    }

    async saveResults(req, res) {
        try {
            const { roomId, playerName, survivalTime } = req.body;
            
            console.log(`[ResultController] Saving result for ${playerName} in ${roomId}`);
            
            await this.resultService.saveResult(roomId, playerName, survivalTime);

            res.json({ success: true });
        } catch (error) {
            console.error('[ResultController] Error saving results:', error.message);
            res.status(400).json({ error: error.message });
        }
    }
}

module.exports = ResultController;
