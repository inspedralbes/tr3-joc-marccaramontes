## 1. Setup & Package Management

- [x] 1.1 Install the **Netcode for GameObjects** (`com.unity.netcode.gameobjects`) package.
- [x] 1.2 Install the **Relay** (`com.unity.services.relay`) and **Core** (`com.unity.services.core`) packages.
- [ ] 1.3 Create a new `NetworkManager` GameObject in the entry scene and configure it with `UnityTransport`.

## 2. Core Infrastructure Refactoring

- [x] 2.1 Refactor `NetworkManager.cs` to act as a wrapper for NGO's `NetworkManager.Singleton`.
- [ ] 2.2 Implement a `RelayManager` (or integrate into `NetworkManager`) to handle Relay allocation and Join Code generation.
- [ ] 2.3 Remove or comment out `NativeWebSocketClient.cs` and the old JSON packet handling logic.

## 3. Prefab & Component Migration

- [ ] 3.1 Modify the Player prefab: Replace `NetworkIdentity` with `NetworkObject`, add `NetworkTransform`, and update `PlayerMovement` to inherit from `NetworkBehaviour`.
- [ ] 3.2 Modify Enemy prefabs: Add `NetworkObject` and `NetworkTransform`. Update `Enemy` to inherit from `NetworkBehaviour`.
- [ ] 3.3 Register Player and Enemy prefabs in the `NetworkManager`'s NetworkPrefabs list.

## 4. Gameplay Logic Migration

- [x] 4.1 Update `PlayerMovement.cs` to use `IsOwner` for input gating and movement logic.
- [x] 4.2 Update `PlayerShooting.cs` to use a `[ServerRpc]` for instantiating bullets, ensuring they are spawned via `NetworkObject.Spawn()`.
- [x] 4.3 Update `EnemySpawner.cs` to check `IsServer` (Host) before spawning and use `NetworkObject.Spawn()` for all enemies.
- [x] 4.4 Implement a `NetworkVariable<float>` for the match timer in `GameManager` to ensure all clients see the same survival time.

## 5. UI & Flow Integration

- [x] 5.1 Update the Main Menu / Lobby UI to include an input field for the Relay Join Code.
- [x] 5.2 Implement the "Host" flow: Request Relay allocation, get Join Code, and start NGO Host.
- [x] 5.3 Implement the "Join" flow: Take Join Code, resolve Relay parameters, and start NGO Client.
- [x] 5.4 Update the `GameManager` to send the match results to the Node.js `api-service` only from the Host instance.

## 6. Testing & Validation

- [ ] 6.1 Verify successful connection and player spawning in a local Host-Client setup.
- [ ] 6.2 Test Relay connectivity using the Join Code between two different network environments.
- [ ] 6.3 Validate that enemy movement and spawning are consistent and smooth across all clients.
