# LoanStatusWebAPI
Loan Status API Development using c#

## Deployment Plan

The LoanStatus Web API Application runs on Microsoft SQLserver file database

### Requirements
1. Application packages (.dlls) 

2. Database sql file to create the MSSQL database and tables (Customer Table, User Table, Loan Table, and Logtrails Table) on the database server.

### Steps
1. Create a folder on the server, copy and paste the application packages in the folder.
2. Create a site on the IIS server and point it to the path of the created folder in step 1
3. Open the sql file in the MSSQL studio database and run it to create the database and the tables
3. Change the connection strings in the application settings file.