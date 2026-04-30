## MODIFIED Requirements

### Requirement: Global UI Fade In/Out
The system SHALL provide a unified method for fading UI panels smoothly using Alpha transparency, ensuring it does not block the caller if initialization is pending.

#### Scenario: Non-blocking Fade Attempt
- **WHEN** a fade is requested and the `UIAnimationManager` is not yet fully initialized (Singleton NULL)
- **THEN** the system MUST log a warning and proceed with instant transparency change to avoid hanging coroutines
