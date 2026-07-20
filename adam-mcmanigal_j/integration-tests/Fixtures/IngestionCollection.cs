using Xunit;

namespace integration.Fixtures;

/// <summary>
/// Collection for the end-to-end ingestion tests, which need all three dependencies at once: the
/// Moto-backed S3 + SQS (<see cref="MotoFixture"/>), a migrated SQL Server
/// (<see cref="SqlServerFixture"/>), and the applied EdiFabric license
/// (<see cref="EdiFabricLicenseFixture"/>). A single collection definition can carry several
/// collection fixtures, which is how one test class receives all three — xUnit allows only one
/// <c>[Collection]</c> per class. Test classes opt in with <c>[Collection("Ingestion")]</c>.
/// </summary>
/// <remarks>
/// This collection instantiates its own Moto and SQL Server containers, separate from the
/// <c>Moto</c>/<c>SqlServer</c> smoke-test collections; xUnit scopes a collection fixture to its
/// defining collection. That is the accepted cost of needing both containers in one test class.
/// </remarks>
[CollectionDefinition("Ingestion")]
public sealed class IngestionCollection
    : ICollectionFixture<MotoFixture>,
        ICollectionFixture<SqlServerFixture>,
        ICollectionFixture<EdiFabricLicenseFixture>;
