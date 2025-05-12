## To-Do List Console App

## Overview
This is a To-Do List Console App designed with a Clean Architecture approach, ensuring modularity, scalability, and testability. 
The application supports CRUD operations for managing tasks, including filtering, sorting, and marking tasks as complete or incomplete.

## 🚀 Building & Running the Application

## Prerequisites
- .NET 6 or later installed (`dotnet --version` to check)
- Visual Studio or VS Code
- Ensure `appsettings.json` is configured properly

## Design Choices
# Backend Architecture
The app follows Clean Architecture, with three layers:

Domain Layer: Contains core business models (ToDoItem, IToDoRepository).
Application Layer: Holds business logic and interface definitions.
Infrastructure Layer: Implements file interactions.

Used Dependency Injection (DI) for service management, ensuring loose coupling and testability.

## Testing Strategy
Unit Tests (Mocked Repository with xUnit)
Integration Tests (Validates persistence in JSON files) 
Dependency Injection (Ensures test isolation)

## Trade-offs Due to Time Constraints
Due to time constraints, I had to skip the following:

No logging was implenented. I wanted to use Serilog for logging and save it a file
I would also add try catch blocks to handle exceptions and log them.

## TESTING INSTRUCTIONS
1. Open Test Explorer
In Visual Studio, go to:

Test > Test Explorer
Test Explorer will show a list of discovered test methods.

2. Run Existing Test Methods(Persistence tests and Non Persistence tests available)
Create the folder `C:\ToDoList` if it does not exist.
In Test Explorer, locate your test methods.
Click Run All to execute all tests.
To run a specific test, right-click on it and select Run.

3. Debug a Test Method
Right-click on the test method.
Select Debug to step through the test execution.
Use breakpoints to inspect variable values and flow.

4. View Test Results
After running, check:
Passed tests (✅ Green)
Failed tests (❌ Red)
Click on a failed test to view error details and some output logging.
Click on a passed test to view sucess details and some output logging.

5. Check json file for tasks output
FilePath `C:\ToDoList\ToDoTasks.json`
