# dynamic-ui-registration Specification

## Purpose
TBD - created by archiving change fix-death-screen-visibility. Update Purpose after archive.
## Requirements
### Requirement: Lazy Registration Fallback
The `GameManager` SHALL attempt to find and register a `ResultsUIRegisterer` component if one has not been registered by the time `ShowResults` is called.

#### Scenario: Fallback during death processing
- **WHEN** `ShowResults()` is called and no `resultsPanel` reference exists
- **THEN** the `GameManager` MUST search the active scene for a `ResultsUIRegisterer` and register it before proceeding

