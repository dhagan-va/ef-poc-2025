# Migration commands

## For HIPAA_5010_837P_Context
```
dotnet ef migrations add HIPAA_5010_837P --context HIPAA_5010_837P_Context --output-dir HIPAA_5010_837P
dotnet ef database update --context HIPAA_5010_837P_Context
```

# Drop and Remove
```
dotnet ef database drop --context HIPAA_5010_837P_Context -f
dotnet ef migrations remove --context HIPAA_5010_837P_Context -f
```
