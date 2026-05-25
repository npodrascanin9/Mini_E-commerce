# Mini E-commerce project
- A simple mini e-commerce project
- Stack: .NET 8, Angular 16, MS SQL Server

### Database tables (MS sql)
<img width="1343" height="417" alt="image" src="https://github.com/user-attachments/assets/ee0e344d-b768-4e71-bd08-65e23f6ebdf8" />


### Backend
- backend (vertical slice architecture)
- Nunit framework for unit/integration tests

# Technical instructions
### Backend
1) Update appsettings.json file (connectionString, DefaultDb value)
2) In the terminal write:
  - Add-Migration InitCreate -OutputDir Database/Migrations
  - Update-Database
3) Verify the ms sql database


### Frontend
1) Make sure that Angular version 14 is installed
2) Run "npm i"
3) Run command "ng s"
