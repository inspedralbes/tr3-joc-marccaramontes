## MODIFIED Requirements

### Requirement: Local Player Self-Identification
The `PlayerMovement` system SHALL automatically identify as the "Local Player" by default. This ensures that the local instance always has authority to process input and physics, while remote "ghost" instances must be explicitly marked as non-local during their initialization.

#### Scenario: Authority in all modes
- **WHEN** the `PlayerMovement` component initializes
- **THEN** the `NetworkIdentity` component MUST set `isLocalPlayer` to true by default, regardless of whether a network manager is present.

#### Scenario: Remote proxy de-authorization
- **WHEN** the `RemotePlayerManager` instantiates a player prefab for a rival
- **THEN** it MUST immediately set `isLocalPlayer` to false on that instance to prevent input/physics conflicts.
