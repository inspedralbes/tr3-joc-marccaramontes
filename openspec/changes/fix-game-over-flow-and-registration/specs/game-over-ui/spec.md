## MODIFIED Requirements

### Requirement: Button Auto-Registration
The system SHALL ensure that navigation buttons in the results panel are automatically linked to the `GameManager` logic as early as possible in the object lifecycle.

#### Scenario: Awake-time UI Registration
- **WHEN** the `ResultsUIRegisterer` awakes
- **THEN** it MUST attempt to register with the `GameManager` immediately, and if unsuccessful, retry using a robust loop until death processing starts
