# .NET 8 Web API – CQRS

This project is a .NET 8 Web API built using CQRS, MediatR, and FluentValidation.
It separates read and write operations and uses two different databases to clearly demonstrate the CQRS pattern.

## Tech Stack

.NET 8

- ASP.NET Core Web API

- CQRS

- MediatR

- FluentValidation

- Entity Framework Core

- Separate Read & Write Databases

## Architecture Overview

- The application follows CQRS (Command Query Responsibility Segregation) with a clear separation between reads and writes.

### Commands (Write Side)

- Handle write operations (Create, Update, Delete)

- Use the Write Database

- Executed via MediatR

- Validated using FluentValidation

- Publish domain events after successful writes

### Queries (Read Side)

- Handle read-only operations

- Use the Read Database

- Optimized strictly for data retrieval

### Events & Projections

- The system uses events and projections to keep the read database in sync with the write database:

- When a command successfully writes data to the Write Database, a domain event is triggered

- Events are handled asynchronously via event handlers

- Projection handlers contain the logic to:

- Transform event data into read models

- Persist those models into the Read Database

### This ensures:

- Loose coupling between write and read models

- Clear separation of responsibilities

- A scalable and extensible CQRS workflow
