/**
 * @interface IResultRepository
 */
class IResultRepository {
    async save(result) { throw new Error('Method not implemented'); }
    async getTopResults(limit) { throw new Error('Method not implemented'); }
    async getResultsByPlayer(username) { throw new Error('Method not implemented'); }
}

module.exports = IResultRepository;
