## ADDED Requirements

### Requirement: Unity Netcode Core Setup
The system SHALL utilize the **Unity Netcode for GameObjects (NGO)** package to manage all multiplayer communication. A single `NetworkManager` instance MUST be present in the boot scene, configured with the `UnityTransport` component.

#### Scenario: NetworkManager Initialization
- **WHEN** the application starts and the Main Menu loads
- **THEN** a persistent `NetworkManager` instance MUST be initialized and ready for Host or Client connection attempts

### Requirement: Networked Prefab Registration
All entities that exist on the network (Players, Enemies, Projectiles) MUST be registered as **NetworkPrefabs** in the `NetworkManager`'s configuration. Each prefab MUST contain a `NetworkObject` component.

#### Scenario: Spawning a Networked Entity
- **WHEN** the Host calls `NetworkObject.Spawn()` on an instantiated networked prefab
- **THEN** that entity MUST be automatically replicated across all connected clients with a consistent `NetworkObjectId`

### Requirement: Authority Transition
The system SHALL adopt a **Host-Authoritative** model. The player who initiates the session acts as the Server and Client (Host), possessing authority over game logic (enemy spawning, wave management, result validation).

#### Scenario: Host Authority Verification
- **WHEN** an enemy is destroyed in a match
- **THEN** only the Host instance SHALL have the authority to process the destruction and update the global kill count
