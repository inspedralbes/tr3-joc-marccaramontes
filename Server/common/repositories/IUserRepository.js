/**
 * @interface IUserRepository
 */
class IUserRepository {
    async findById(id) { throw new Error('Method not implemented'); }
    async findByUsername(username) { throw new Error('Method not implemented'); }
    async save(user) { throw new Error('Method not implemented'); }
    async updateStats(username, stats) { throw new Error('Method not implemented'); }
}

module.exports = IUserRepository;
