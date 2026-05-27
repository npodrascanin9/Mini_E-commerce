# Mini E-commerce Project
- A simple mini e-commerce project
- **Stack:** .NET 8, Angular 14, MS SQL Server

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
The frontend application is built with **Angular 14** and provides a simple interface for interacting with the API.

### Routes
- **Product Categories** – full CRUD operations (create, read, update, delete)  
- **Products** – CRUD operations, including:
  - **Export to Excel** (using ClosedXML on the backend)  
  - **Articles management** – when updating a product, articles can be added/edited/removed  

### Libraries & Packages
- **Angular Material** – UI components (tables, forms, datepicker, etc.)  
- **RxJS** – reactive programming for handling asynchronous data streams  
- **TypeScript** – strongly typed language for building scalable frontend logic  

### UI Preview

#### Product Categories
The **Product Categories** route supports full CRUD operations:
- **Create** – add a new category  
- **Read** – view all categories in a table  
- **Update** – edit an existing category  
- **Delete** – remove a category record  

###### Screenshots
1) Read
<img width="1918" height="1127" alt="image" src="https://github.com/user-attachments/assets/d2dffb7d-48fb-4be2-8019-ef847c4ab2df" />

2) Create
2.1) Dialog for inserting a record
<img width="1918" height="1066" alt="image" src="https://github.com/user-attachments/assets/a795f82e-6495-47aa-882c-cd45f97dcb5d" />

2.2) Validation error for preventing a user to insert a category that already exists
<img width="1916" height="1010" alt="image" src="https://github.com/user-attachments/assets/9e048f97-2fc6-4495-adc7-0e07679dc046" />

3) Update
3.1) Dialog for editing a record
<img width="1918" height="1048" alt="image" src="https://github.com/user-attachments/assets/87394f1f-20f6-48db-83f9-9c15f0c39c9b" />

3.2) Preview of data after updating a record
<img width="1918" height="1017" alt="image" src="https://github.com/user-attachments/assets/7553103f-8d3a-43b4-96a1-fd22a37a87a9" />


5) Delete
5.1) Confirmation
<img width="1918" height="887" alt="image" src="https://github.com/user-attachments/assets/5cb5300c-0476-45d9-8e27-f96016dff9f0" />

5.2) Verify that deleted record is not listed (after deleting a record, records will be fetched again)
<img width="1917" height="1021" alt="image" src="https://github.com/user-attachments/assets/d35cf5dc-6aee-4cb3-9be0-279ad8bfe1b8" />


