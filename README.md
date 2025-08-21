# CashFlow

## About the Project

**API** developed using **.NET 8**, follows the principles of **Domain-Driven Design (DDD)** to provide a structured and efficient solution for managing personal expenses. The main goal is to allow users to record their expenses, detailing information such as title, date and time, description, amount, and payment type, with the data securely stored in a **MySQL** database.

The **API** architecture is based on **REST**, using standard **HTTP** methods for efficient and simplified communication. Additionally, it includes **Swagger** documentation, offering an interactive graphical interface for developers to explore and test the endpoints easily.

Among the NuGet packages used, **AutoMapper** handles the mapping between domain and request/response objects, reducing the need for repetitive and manual code. **FluentAssertions** is used in unit tests to make assertions more readable, helping to write clear and understandable tests. For validations, **FluentValidation** is used to implement rules in a simple and intuitive way within request classes, keeping the code clean and easy to maintain. Finally, **EntityFramework** acts as an ORM (Object-Relational Mapper) that simplifies database interactions, allowing the use of .NET objects to manipulate data directly without having to deal with raw SQL queries.

## Features

- **Domain-Driven Design (DDD)**: Modular structure that makes the application's domain easier to understand and maintain.
- **Unit Testing**: Comprehensive tests using FluentAssertions to ensure functionality and quality.
- **Report Generation**: Ability to export detailed reports to **PDF and Excel**, providing effective visual analysis of expenses.
- **RESTful API with Swagger Documentation**: Documented interface that simplifies integration and testing for developers.

## Built With

![dotnet-badge]
![jetbrains-rider-badge]
![docker-badge]
![swagger-badge]

## Quick Start

### Requirements

* Windows, Linux or macOS with the [.NET SDK][dotnet-sdk] installed
* Visual Studio 2022+, Visual Studio Code or JetBrains Rider
* MySQL

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/matheushfp/cashflow.git
    ```

2. Fill in the necessary details in the `appsettings.Development.json` file.
3. Run the API and start testing it out! :)

<!-- Links -->
[dotnet-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

<!-- Badges -->
[dotnet-badge]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[jetbrains-rider-badge]: https://img.shields.io/badge/Rider-000?logo=rider&logoColor=fff&style=for-the-badge
[docker-badge]: https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=fff&style=for-the-badge
[mysql-badge]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[swagger-badge]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
