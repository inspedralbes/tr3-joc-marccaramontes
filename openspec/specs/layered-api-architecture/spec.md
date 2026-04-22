# layered-api-architecture Specification

## Purpose
Defines the Controller-Service-Repository pattern for all microservices to ensure clear separation of concerns, technology-agnostic business logic, and testability.

## Requirements

### Requirement: Controller-Service-Repository Layered Structure
All microservices SHALL be structured into three distinct layers: Controller, Service, and Repository. This applies to both HTTP-based and WebSocket-based services.

#### Scenario: Request handling flow
- **WHEN** an HTTP request OR a WebSocket message is received by the Gateway
- **THEN** it is routed to the corresponding Controller in the respective Service, which validates the input and delegates business logic to a Service, which in turn uses a Repository for data persistence.

### Requirement: Technology-Agnostic Services
Services SHALL interact with Repositories only through defined interfaces, remaining completely unaware of the underlying persistence technology (e.g., PostgreSQL, InMemory).

#### Scenario: Repository injection
- **WHEN** the API Service starts
- **THEN** the concrete Repository implementation is instantiated in `index.js` and injected into the Service constructor.

### Requirement: Centralized Business Logic in Service Layer
All business logic, including validations, data transformations, and security operations (like password hashing), SHALL reside exclusively within the Service layer.

#### Scenario: Password security
- **WHEN** a new user is created
- **THEN** the Service layer hashes the password before passing the user entity to the Repository.
