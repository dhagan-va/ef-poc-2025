# Quick start

## Create a .env file in archambault-jacob_w/src
```
cd <project location>/ef-poc-2025/archambault-jacob_w/src
echo -e "ACCEPT_EULA=Y\nMSSQL_SA_PASSWORD=<Your password here>" > .env
```

## Start a SQL Server instance:
`docker compose up`
or more verbosely:
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<password>" \
   -p 1433:1433 --name sql1 \
   -d \
   mcr.microsoft.com/mssql/server:latest
```
## Install .NET:
### Install the .NET 9 SDK:
```
curl -fsSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.100 --install-dir $HOME/.dotnet
[[ ":$PATH:" != *":$HOME/.dotnet:"* ]] && export PATH="$PATH:$HOME/.dotnet"
```

### Install Entity Framework:
`dotnet tool install --global dotnet-ef`

##  Run EF migration:
`cd <project location>/ef-poc-2025/archambault-jacob_w/src/EFPOC.DB`
`dotnet ef database update`
Check that initial migration was successful:
- Check that database creation was successful: `docker exec -it sql1 /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P '<password>' -Q "SELECT name FROM sys.databases;" -No`
- Check that tables were created: `docker exec -it sql1 /opt/mssql-tools18/bin/sqlcmd -S 127.0.0.1 -U SA -P '<password>' -d HIPAA -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES" -No`

## Start project:
`cd ef-poc-2025/archambault-jacob_w/src/EFPOC.Console`
`dotnet run`

# About

# Prerequisites
## SQL Server install
This project requires a running instance of SQL Server. 

### Option 1: Local SQL Server install
To install SQL Server on your computer, you can follow the instructions at https://learn.microsoft.com/en-us/sql/database-engine/install-windows/install-sql-server?view=sql-server-ver17

### Option 2: Using Docker
Alternately, you can run SQL Server in a Docker container. 

To pull down the docker image, run `docker pull mcr.microsoft.com/mssql/server:2025-latest`, 

To get started with creating a container instance, run `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<password>" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d \
   mcr.microsoft.com/mssql/server:2025-latest`, replacing <password> with a password of your choosing.

To test your database connection, you can run 
```
docker exec -it sql1 /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P '<password>' -Q "SELECT name FROM sys.databases;" -No
```
If successful, the above command should run a query as the system administrator (`-U SA`), without encryption (`-No` if this option is not provided, a self-signed certificate error message is returned), that returns the names of the system databases that come with your instance.

For more information, see [here](https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver17&tabs=cli&pivots=cs1-bash).

### Option 3: Using Podman
Podman is a rootless, daemonless alternative to Docker that comes pre-installed on most Red Hat, CentOS, and Fedora linux systems whose command line interface is nearly identical with Docker's. To install with Podman, follow the instructions above for a Docker install, simply substituting the term 'podman' for 'docker'.

## Run entity framework migrations
This project uses entity framework migrations for its database setup and updates. [Entity framework](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli) is an object relational mapper that maps SQL Server relations (tables) to C# classes. This allows SQL Server data to be converted into C# objects and manipulated in memory accordingly, and conversely allows instances of C# classes to be persisted as SQL data.

### About entity framework migrations
Entity framework migrations are a [code first](https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database) technique for building and updating databases from schemata defined in code. A code first approach is contrasted with a database first approach, which sets up database schema independently and prior to building applications that must interface with it. By using entity framework migrations, we can automatically seed a database from the schemata that we define via C# classes, and update SQL Server tables simply by updating our corresponding model class definitions and running a migration.

A *migration* is a plan for incrementally updating a database, represented as C# code, checking C# entity models to determine if they have been changed since the last migration and updating  table schemata accordingly. It represents a blueprint for an update, not an update itself. 

To create a migration open a command line and run `dotnet ef migrations add <migration name>` in a terminal at the root of your C# database project, providing a name describing the changes that the migration stages. 

To update your database with your migration changes, run `dotnet ef database update` from the root of your db project.
  
# How to run this project
