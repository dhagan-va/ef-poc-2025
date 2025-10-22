dotnet ef migrations add InitialCreate
dotnet ef database update

dotnet ef migrations add AddUniqueClaimIndex
dotnet ef database update
