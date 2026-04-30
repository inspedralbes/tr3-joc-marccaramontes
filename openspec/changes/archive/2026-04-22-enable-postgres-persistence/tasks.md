## 1. Database Infrastructure

- [x] 1.1 Create `Server/common/scripts/schema.sql` with `users` and `results` table definitions.
- [x] 1.2 Ensure `pg` library is available in the `Server` workspace.

## 2. Repository Factory Implementation

- [x] 2.1 Create `Server/common/repositories/RepositoryFactory.js` to encapsulate switching logic between In-Memory and PostgreSQL.
- [x] 2.2 Implement database connection health check in the factory to support graceful fallback.

## 3. Repository Refinement

- [x] 3.1 Update `PostgresUserRepository.js` to ensure `updateStats` correctly merges JSONB data.
- [x] 3.2 Update `PostgresResultRepository.js` to include `timestamp` in the `save` method and schema.

## 4. Service Integration

- [x] 4.1 Refactor `Server/api-service/index.js` to use `RepositoryFactory` for initializing `userRepo` and `resultRepo`.
- [x] 4.2 Refactor `Server/game-service/index.js` to use `RepositoryFactory` for initializing `userRepo`.

## 5. Validation

- [x] 5.1 Verify that services start successfully using In-Memory repositories when `DATABASE_URL` is missing.
- [x] 5.2 Verify that services correctly connect and persist data to PostgreSQL when `DATABASE_URL` is provided.
