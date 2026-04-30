## ADDED Requirements

### Requirement: Infernal Aesthetic Main Menu
The system MUST provide a main menu scene with a solid black background (#000000) and high-contrast red UI elements.

#### Scenario: Visual Consistency
- **WHEN** the Main Menu scene is loaded
- **THEN** the background is black and the primary text color is #FF0000 (Red)

### Requirement: Reactive Central Circle
The system SHALL display a central circular element representing the player that performs a breathing animation (scaling via sine wave).

#### Scenario: Breathing Animation
- **WHEN** the menu is idle
- **THEN** the central circle smoothly scales between 95% and 105% of its original size

### Requirement: Button Disintegration Feedback
The system SHALL trigger a pixel-based disintegration effect using the `PixelExplosion` system when a menu option is selected.

#### Scenario: Disintegration on Select
- **WHEN** a menu button is clicked
- **THEN** the button text is hidden and a particle burst of red square particles is spawned at its position
