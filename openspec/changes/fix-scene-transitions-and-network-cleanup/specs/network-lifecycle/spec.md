## ADDED Requirements

### Requirement: Event Unregistration
All components that subscribe to `NetworkManager` events MUST unregister from those events when the component is disabled or destroyed.

#### Scenario: Object Destruction
- **WHEN** a GameObject with a network-listener component is destroyed (e.g., scene change)
- **THEN** the component SHALL remove its listeners from the `NetworkManager` static events using the `-=` operator.

### Requirement: Room Exit Notification
The client MUST notify the server when leaving a networked room to ensure the player list is updated.

#### Scenario: Returning to Menu
- **WHEN** the user clicks the "Return to Menu" button during an active session
- **THEN** the `NetworkManager` SHALL emit a "leave_room" event to the server before the scene transition occurs.
