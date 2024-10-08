# Microservices App

## Overview

This repository contains a microservices application that is currently under development. The application is designed with a focus on SOLID principles, Command Query Responsibility Segregation (CQRS), and the MediatR pattern. The goal is to create a scalable and maintainable system using modern architectural practices.

## Architecture

### SOLID Principles

The application is built adhering to SOLID principles, ensuring a maintainable and scalable codebase:

- **Single Responsibility Principle (SRP)**: Each service and class has a single responsibility, enhancing clarity and reducing complexity.
- **Open/Closed Principle (OCP)**: Services and components are open for extension but closed for modification.
- **Liskov Substitution Principle (LSP)**: Subtypes can replace their base types without altering the correctness of the program.
- **Interface Segregation Principle (ISP)**: Interfaces are designed to be client-specific, avoiding unnecessary dependencies.
- **Dependency Inversion Principle (DIP)**: High-level modules depend on abstractions rather than concrete implementations.

### CQRS (Command Query Responsibility Segregation)

The application employs CQRS to separate read and write operations, improving scalability and performance:

- **Commands**: Responsible for making changes to the system. Commands are handled by dedicated command handlers.
- **Queries**: Responsible for reading data. Queries are handled by separate query handlers.

### MediatR Pattern

MediatR is used to implement the mediator pattern, providing a way to send requests and handle them without tight coupling between components:

- **Request/Response**: Defines requests and their corresponding handlers, promoting a clear separation between requests and their handling logic.
- **Behaviors**: Implements cross-cutting concerns such as logging and validation in a centralized manner.

## Getting Started

### Prerequisites

- .NET 7.0 or later
- Docker (for containerization)
- RabbitMQ (for messaging, if applicable)


