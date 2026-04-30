## 1. Networking Infrastructure Updates

- [x] 1.1 Add `ENEMY_SYNC` event to `NetworkManager.cs` (Handle received event and provide a way to emit it)
- [x] 1.2 Update DTOs in `NetworkManager.cs` for agent state synchronization (ID, position, etc.)
- [x] 1.3 Update the `SocketController` in the `game-service` to broadcast `ENEMY_SYNC` events to the room

## 2. Hunter Agent Logic Refinement

- [x] 2.1 Refactor `HunterAgent.cs` to include a `FindClosestPlayer` method
- [x] 2.2 Update `OnActionReceived` and `CollectObservations` to use the dynamically found closest player as the target
- [x] 2.3 Ensure the agent logic only runs on the Host (ML-Agents `DecisionRequester` enabled only on Host)

## 3. Network Synchronization Improvement

- [x] 3.1 Refactor `HunterAgentNetworkSync.cs` to use the new `ENEMY_SYNC` event instead of `SPAWN_ENEMY`
- [x] 3.2 Implement `Vector3.Lerp` interpolation in `HunterAgentNetworkSync.cs` for remote clients to smooth movement
- [x] 3.3 Ensure Host-only emission of position updates to all clients via `NetworkManager.Emit`

## 4. Integration and Validation

- [x] 4.1 Verify hunter agent's target switching logic in a multi-player session
- [x] 4.2 Validate smooth movement in remote clients during a match
- [x] 4.3 Ensure the agent correctly rewards the Host and resets episodes on collision
