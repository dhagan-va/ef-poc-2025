using test.Parsing;
using Xunit;

namespace test;

/// <summary>
/// Groups every test that needs the EdiFabric license into a single xUnit collection so the
/// license is applied exactly once, through one shared <see cref="EdiFabricLicenseFixture"/>.
/// EdiFabric's <c>SerialKey.Set</c> is global and validates over the network; applying it
/// per-class in parallel makes concurrent validations fail with "The serial key is invalid!".
/// Test classes opt in with <c>[Collection("EdiFabric")]</c>.
/// </summary>
[CollectionDefinition("EdiFabric")]
public sealed class EdiFabricCollection : ICollectionFixture<EdiFabricLicenseFixture>;
