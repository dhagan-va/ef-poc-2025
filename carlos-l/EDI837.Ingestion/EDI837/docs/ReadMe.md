# Instructions On How to set up the project. 
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