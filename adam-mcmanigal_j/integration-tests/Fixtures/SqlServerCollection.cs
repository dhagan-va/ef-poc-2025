using Xunit;

namespace integration.Fixtures;

/// <summary>
/// Shares a single <see cref="SqlServerFixture"/> (one migrated SQL Server container) across every
/// test class that needs the database. Test classes opt in with <c>[Collection("SqlServer")]</c>.
/// </summary>
[CollectionDefinition("SqlServer")]
public sealed class SqlServerCollection : ICollectionFixture<SqlServerFixture>;
