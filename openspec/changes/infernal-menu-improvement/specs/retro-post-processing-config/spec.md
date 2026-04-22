## ADDED Requirements

### Requirement: Retro Visual Filters
The system SHALL apply aggressive post-processing effects to the Menu scene to simulate a low-resolution, high-contrast 90s aesthetic.

#### Scenario: Post-Processing Stack
- **WHEN** the Menu scene is active
- **THEN** Film Grain, Bloom (high intensity), and Vignette effects are visible

### Requirement: Blood Bloom Effect
The system MUST ensure that red UI elements emit a glow (Bloom) that bleeds into the surrounding black background.

#### Scenario: Red Glow
- **WHEN** a red UI element is displayed
- **THEN** it shows a glowing aura with a threshold that allows pure red to bleed
