dotnet ef migrations add InitialCreate --context HIPAA_5010_837P_Context
dotnet ef database update --context HIPAA_5010_837P_Context

dotnet ef migrations add InitialCreate --context ClaimStagingContext
dotnet ef database update --context ClaimStagingContext
