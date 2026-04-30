## MODIFIED Requirements

### Requirement: Local Player Self-Identification
The `PlayerMovement` system SHALL automatically identify as the "Local Player" or "Owner" using the NGO framework. This ensures that the local instance always has authority to process input, while remote "ghost" instances are controlled by network updates.

#### Scenario: Authority in all modes
- **WHEN** the `PlayerMovement` component initializes as part of an NGO session
- **THEN** it MUST check `IsOwner` and `IsLocalPlayer` from its `NetworkBehaviour` to determine if it should process input.

#### Scenario: Remote proxy de-authorization
- **WHEN** the NGO system instantiates a player prefab for a rival
- **THEN** it MUST automatically set `IsOwner` to false on that instance for the local client, which SHALL disable local input processing scripts.
