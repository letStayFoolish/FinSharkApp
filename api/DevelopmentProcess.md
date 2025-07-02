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
## Data Validation

### Data validation for URL and JSON (_from body_)

For simple types, where we pass `string` instead of `int`:
```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetById([FromRoute] int id) {}
```
This should not pass the validation, since we typed `id` as an `int` type value.
```csharp
[HttpGet("{id: int}")]
public async Task<IActionResult> GetById([FromRoute] int id) {}
```

### Complex Forms of validation

Inside DTOs, put **Data Validation Annotations** on top of our actual properties.

Recommendation: Never put Data Validation Annotations inside a real model! It will apply globally.
`[Required]`, `[MinLength(int, ErrorMessage = string)]`, `[MaxLength(int, ErrorMessage = string)]`, `[Range(1, 1000)]`

## Filtering

`ToList()` is very important, because it is what generates an SQL - pretty much it fires a gun to the db to get data back.

To defer this, we have `AsQueryable()` will delay firing SQL gun - so we can do filtering, limiting, or something else before we get the data.

## Pagination

Using `Skip(int)` and `Take(int)` in combination.

## Token Services
### Claims vs. Roles

**Roles** are more generic and old school.

**Claims** don't require DB and very flexible.

Microsoft has moved away from Roles.

Claims are (just as Roles) key-value pairs of things that going to describe of what a user does or what a user can do.

Flow:

- When user do Login, submitting email and password - sending JWT to the server;
- Claims Principal: when User is authenticated, Claims Principal is going to be created;
- From Claims Principal we are going to get information about User such: email, timeZone, etc...

Making `Service` folder. `Repository` folder is for DB calls, while on the other hand `Service` is for any kind of abstraction.

## `IEnumerable<T>`

`IEnumerable<T>` is an interface in C#. It represents a **collection of elements that can be enumerated (iterated over)**, one at a time, in a forward-only manner. Essentially, it serves as a base type for any sequence of data, like arrays, lists, or queries like those you see with LINQ.

**Some Key Facts About `IEnumerable<T>`:**

1. **Forward-Only Iteration:** You can use a `foreach` loop to iterate through items, but you can't move backward or access elements by index directly (like you can with a `List<T>` or array).
2. **Lazy Elevation:** When used with LINQ queries (`AsQueryable()` in my code), it **doesn't execute (or fetch data)** until you explicitly need the data, for example, calling `ToList()` or enumerating it in a loop. This is efficient for dealing with large data or querying databases.
3. **Common Base:** Many collection types (e.g. `List<T>`, `Array`, `Dictionary<T>`, LINQ queries against EF Core) implement `IEnumerable<T>`.

**When to Use `IEnumerable<T>` Over Other Types?**

1. **Return Type for Read-Only Iteration:** If you are writing a method that only needs to **return a sequence for iterating** without modifying it, prefer `IEnumerable,T>`. For example:
```csharp
public IEnumerable<int> GetNumbers() {
    return new List<int> {1, 2, 3, 4 };
}
```
This ensures consumers of your method can only iterate through the collection, not manipulate it directly (like adding/removing items).

2. **Deferred Execution:** It is perfect for **querying data** from a database or any source where results shouldn't be materialized (fetched) immediately. In your example, the `_context.Stocks.AsQueryable()` part ensures the query is deferred. It fetches data from the database only when necessary (like when you call `ToListAsync()`).
3. **Abstraction for Flexibility:** When you want to return a sequence but don't want to commit to a particular type (`List<T>`, `Array`, etc...), use `IEnumerable<T>`. It allows you to change implementations later without breaking the contract.

**When Not to Use `IEnumerable<T>`?**

- If you need to **modify the collection**, such as adding/removing elements, use `List<T>` or another mutable collection type.
- If random access by index is required (like `list[i]`) use `List<T>` or an `array` because `IEnumerable<T>` does not support this.
- If you need advanced operations like sorting or searching, `IEnumerable,T>` might not be the best choice; work with more specific type instead.