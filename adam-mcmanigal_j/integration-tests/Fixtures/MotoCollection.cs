using Xunit;

namespace integration.Fixtures;

/// <summary>
/// Shares a single <see cref="MotoFixture"/> (one Moto container + one provisioning pass) across
/// every test class that needs S3/SQS. Test classes opt in with <c>[Collection("Moto")]</c>.
/// </summary>
[CollectionDefinition("Moto")]
public sealed class MotoCollection : ICollectionFixture<MotoFixture>;
