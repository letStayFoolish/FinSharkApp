# Important Notes
## These notes represent some of the key things on my path of learning ASP.NET Core

## ORM and Entity Framework

Database tables are not a format that software developer could work with. Thats why we need (and have) ORM.

**ORM** Object Relational Mapper. In .NET we use **Entity Framework** as ORM.

### Why do we need Entity Framework?

What Entity Framework is going to do, is Entity Framework is to take database tables and turn them into objects. One object is represantation of **one row** from the database.

### DTO

**DTO** stands for Data Transfer Object. In the context of .NET Core, and making APIs, it is about Request/Response. Many times we do not want to return everything to the user but limited data. E.g. password.

### POST

Using **Entity Framework**'s method `Add()`. `Add` will not instantly save to database, it is going to start tracking that object. `SaveChanges()` will actually add object to the db.

## Code to an Interface == Code to an abstraction

Very repetitive code turns to abstraction, using **Repository pattern**.

- Instead of tightly coupling your code to a concrete implementation (class), you write code that depends on an **interface** (an abstraction).
- This makes your code **flexible**, **testable**, and **easier to change** in the future.


**Abstraction** - hiding piece of code within another method.

## Repository Pattern
- **What it means**:
    - A design pattern used to separate the **data access logic** (e.g., database queries) from the **business logic** of your application.
    - Instead of directly dealing with database queries inside your controllers/services, you use a repository to **encapsulate common data operations**.

- **Why use it?**:
    - Makes your code more structured and reusable.
    - Makes testing easier (e.g., mock repositories instead of using a real database).


## Dependency Injection (DI)
- **What it means**:
    - **Dependency Injection** is a technique where objects or services (dependencies) are **provided to a class**, rather than the class creating them itself.
    - This allows your code to depend on abstractions (like interfaces) and makes it easier to change implementations or perform tests.

- **Why use it?**:
    - Makes your code more modular, testable, and maintainable.
    - Decouples classes from concrete implementations.

- **How it works**:
    - Register services/interfaces in the **Dependency Injection container**.
    - The container **injects the dependencies** into classes that need them.

- **Analogy**:
    - Imagine a restaurant: You sit at your table (controller), and the waiter (DI container) brings the food (dependency) you ordered. You don’t go to the kitchen (implementation) yourself.

 Example with `CommentController`:
- Interfaces are being injected into the `CommentController` using its constructor;
- The dependencies (`ICommentRepository`, `IStockRepository`) are provided by the DI container when creating an instance of `CommentController`;
- For example:
  ```csharp
  builder.Services.AddScoped<ICommentRepository, CommentRepository>();
  ```
- `AddScoped()` method registers the dependencies in the DI container and ensures that a new instance of the service is created for each HTTP request.
 