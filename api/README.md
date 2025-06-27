# Title

## ORM and Entity Framework

Database tables are not a format that software developer could work with. Thats why we need (and have) ORM.

**ORM** Object Relational Mapper. In .NET we use **Entity Framework** as ORM.

### Why do we need Entity Framework?

What Entity Framework is going to do, is Entity Framework is to take database tables and turn them into objects. One object is represantation of **one row** from the database.

### DTO

**DTO** stands for Data Transfer Object. In the context of .NET Core, and making APIs, it is about Request/Response. Many times we do not want to return everything to the user but limited data. E.g. password.

### POST

Using **Entity Framework**'s method `Add()`. `Add` will not instantly save to database, it is going to start tracking that object. `SaveChanges()` will actually add object to the db.
