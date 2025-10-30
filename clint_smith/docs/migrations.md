# Migration commands
Helpful commands for adding and running migrations

## For HIPAA_5010_837P_Context
```
dotnet ef migrations add InitialCreate_Main --context HIPAA_5010_837P_Context --output-dir Migrations_Main
dotnet ef database update --context HIPAA_5010_837P_Context
```

## For ClaimStagingContext
```
dotnet ef migrations add InitialCreate_Staging --context ClaimStagingContext --output-dir Migrations_Staging\n
dotnet ef database update --context ClaimStagingContext
```

# Drop and Remove
```
dotnet ef database drop --context ClaimStagingContext -f
dotnet ef migrations remove --context ClaimStagingContext -f
```
