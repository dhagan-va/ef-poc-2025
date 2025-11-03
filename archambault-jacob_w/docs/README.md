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

# How to run this project
