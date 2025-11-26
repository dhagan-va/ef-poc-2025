# Instructions on How to set up the project. 
- Project Selection:
	1. Open Visual Studio.
	2. Click on Clone a repository.
	3. For the Repository location, use https://github.com/dhagan-va/ef-poc-2025.git 
	4. Branch your project and create you folder with the following format <name>-<last initial>.
	5. Inside the folder, Add the solution and type of project you want to work with. (Console, Website, webAPI) .
	 
- Install the following packages: 
	1. EdiFabric (For Reading and parsing EDI files)
	2. EdiFabric.Templates.Hippa (For Reading and parsing EDI files)
	3. Microsoft.EntityFrameworkCore (For DB interaction(ORM))
	4. Microsoft.EntityFrameworkCore.SqlServer (For interaction to a specific provider. (SLQ Server) )
	5. Microsoft.EntityFrameworkCore.Tools (For Code First Migrations)

- Register your EdiFrabric key in the .NET Core Pipeline. You can get it here [https://support.edifabric.com/hc/en-us/articles/360000280532-Free-code-to-master-your-EDI-files] 

# Instructions On How to set up a test project.
- Add a Test project as follows:
	1. Select the type of Test project you want to implement (XUnit, NUnit).
- Install the following packages:
	1. Microsoft.EntityFrameworkCore.
	2. Microsoft.EntityFrameworkCore.InMemory (For Creating a in-memory DB.)
	3. Moq (For mocking configurations etc.).
- Use the Setup (NUnit) to mock and setup the necessary settings and services.  
- Create your Tests. 

# Instructions On How to set up a Moto.
- Make sure you have installed Python in your machine (Python Installation will install pip).
- Install Moto.py
```
pip install moto[server]
```
- Moto will be installed under the User. If you are not able to use "moto_server", but instead you 
have to use it as "python -m moto.server", you have to register the user the user path in the in the environment
variables as follows:
1. Use the search in the task bar to look for Environment Variables.
2. Click on Environment Variables.
3. Under System variables select "Path" and Edit. 
4. Click on Add New and add the path to scripts, example:  C:\Users\<your User>\AppData\Roaming\Python\Python314\Scripts
5. Restart your command window and you should be able to use moto_server. 

# Instructions On How to set up a AWS CLI.
- Install AWS CLI as per documentation in the official site [https://docs.aws.amazon.com/cli/v1/userguide/install-windows.html]
- Add the following variables to the Environment Variables under the User variables. 	
	1. AWS_ACCESS_KEY_ID='testing'
    2. AWS_SECRET_ACCESS_KEY='testing'
    3. AWS_SECURITY_TOKEN='testing'
    4. AWS_SESSION_TOKEN='testing'
    5. AWS_DEFAULT_REGION='us-east-1'
- With the variables in place, restart the Command console and now AWS.CLI can interact with MOTO.

# Instructions On How to set up Amazon Web Services for .NET Core.
-  Install the following packages: 
	1. AWSSDK.Core
	2. AWSSDK.Extensions.NETCore.Setup

# Instructions On How to Run the projects.
- There are two files with extension .cmd at the root of the project.
	- MotoStartup has two functions as follows:
		1. Kills and instance of Moto_server.
		2. Starts a new instance of Moto_server.

	- S3Startup has two functions as follows:
		1. Creates the "edi-bucket".
		2. Adds the three files under the samples folder to the edi-bucket.
- There is a PowerShell file, StartUp.ps1 that runs the two cmd files above. The PowerShell
file is executed by the Build Event of the project in the pre-build section. When the project builds,
it creates a AWS Mocking environment with all the necessary elements to execute the API calls and 
the Unit testing. 