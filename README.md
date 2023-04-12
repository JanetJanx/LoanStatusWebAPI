# LoanStatusWebAPI
Loan Status API Development using c#

## Deployment Plan

The LoanStatus Web API Application runs on Microsoft SQLserver file database

### Requirements
1. Application packages (.dlls) 

2. Database sql file to create the MSSQL database and tables (Customer Table, User Table, Loan Table, and Logtrails Table) on the database server.

### Steps
1. Open LoanStatusWebAPI\bin\Release\net7.0 and zip the "publish" folder. Transfer the zipped folder to the deployment server.
2. Create a folder on the server, unzip the pubkished folder, copy and paste the application packages in the created folder.
3. Create a site on the IIS server and point it to the path of the created folder in step 2
4. Open the LoanStatusWebAPL.Data.sql file in the MSSQL studio database and run it to create the database and the tables
5. Change the connection strings in the application settings file.
6. Go back to IIS and start the site