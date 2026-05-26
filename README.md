# Mini E-commerce Project
- A simple mini e-commerce project
- **Stack:** .NET 8, Angular 16, MS SQL Server

# Technical instructions
## Backend
1) Update appsettings.json file (connectionString, DefaultDb value)
2) In the terminal write:
    - Add-Migration InitCreate -OutputDir Database/Migrations
    - Update-Database
3) Verify the ms sql database


## Frontend
1) Make sure that Angular version 14 is installed
2) Run:
   - npm i
   - ng s
3) Verify that localhost:4200 is running wihtout any bugs

## 📝 Database Notes
- The project uses **Entity Framework Core** with a **code‑first approach**.  
- Database schema is automatically generated and updated through migrations — there is **no need to manually create tables**.  
- The only configuration required is updating the `appsettings.json` file:  
  - "DefaultDb" should point to the main database (**CommerceDb**).  
  - In the `Api.IntegrationTests` project, `"DefaultDb"` should reference the testing database (**CommerceTestDb**).  

Recommended sql commands:
    CREATE DATABASE CommerceDb;
    CREATE DATABASE CommerceTestDb;

### Database Schema Preview
<img width="1343" height="417" alt="image" src="https://github.com/user-attachments/assets/ee0e344d-b768-4e71-bd08-65e23f6ebdf8" />



# System Intro

## 1. API
The backend exposes several endpoints:

- `api/products`
- `api/products/{productId}/articles`
- `api/productCategories`
- `api/options/productCategory`

### Architecture
The backend is implemented using **Vertical Slice Architecture**:
- Each feature is self‑contained (commands, queries, handlers, DTOs).
- Business logic is grouped by feature instead of layered across the entire project.
- This approach improves modularity, scalability, and maintainability.


### 📚 Libraries & Packages
- **Entity Framework Core** – ORM with code‑first approach (SQL Server provider + migrations)  
- **MediatR** – CQRS pattern implementation (commands & queries)  
- **FluentValidation** – Input validation  
- **Mapster** – Object mapping between DTOs and entities (specifically used for mapping requests to commands)  
- **ClosedXML** – Excel file export support  
- **Newtonsoft.Json** – JSON serialization/deserialization  
- **FluentAssertions** – Readable assertions for writing unit / integration tests 


### API Launch Preview
When launching the project, Swagger UI is available for testing:
<img width="1898" height="1127" alt="image" src="https://github.com/user-attachments/assets/c154c6ad-3462-4375-a6ca-d6124782d6ef" />

### Test Results Preview
Below is a preview of the test results (unit and integration tests):

<img width="1912" height="1061" alt="image" src="https://github.com/user-attachments/assets/4d867716-3498-4862-8e77-49ad10007fa5" />



## 2. UI
The frontend application is built with **Angular 16** and provides a simple interface for interacting with the API.

### Routes
- **Product Categories** – full CRUD operations (create, read, update, delete)  
- **Products** – CRUD, including export excel, and logic for Articles 

### Libraries & Packages
- **Angular Material** – UI components (tables, forms, datepicker, etc.)  
- **RxJS** – reactive programming for handling asynchronous data streams  
- **TypeScript** – strongly typed language for building scalable frontend logic  

### UI Preview
Below is a preview of the user interface:

----
