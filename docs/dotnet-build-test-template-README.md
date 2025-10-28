# .NET Build and Test Template

A reusable GitHub Actions workflow template for building .NET solutions and running unit tests with comprehensive reporting.

## Features

- ✅ **Multi-version .NET support** - Supports .NET 6, 7, and 8
- ✅ **Flexible configuration** - Customizable build settings and test filters
- ✅ **Unit Tests** - Automatic discovery and execution of unit tests
- ✅ **SpecFlow Tests** - Optional SpecFlow/BDD test execution
- ✅ **Test Reporting** - Detailed test reports with pass/fail status
- ✅ **PR Integration** - Automatic comments on pull requests with build/test status
- ✅ **Code Coverage** - XPlat code coverage collection
- ✅ **GitHub Packages** - Built-in support for GitHub NuGet registry
- ✅ **Artifact Upload** - Test results saved as downloadable artifacts

## Quick Start

### Basic Usage

```yaml
name: Build and Test

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  build-test:
    uses: ./.github/workflows/dotnet-build-test-template.yml
    with:
      SOLUTION_FILE: "MySolution.sln"
    secrets:
      REGISTRY_USER: ${{ secrets.REGISTRY_USER }}
      REGISTRY_TOKEN: ${{ secrets.REGISTRY_TOKEN }}
```

### Advanced Usage

```yaml
name: Complete Build and Test

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  comprehensive-test:
    uses: ./.github/workflows/dotnet-build-test-template.yml
    with:
      SOLUTION_FILE: "MyApp.sln"
      WORKING_DIRECTORY: "src"
      DOTNET_VERSION: "8.x"
      BUILD_CONFIGURATION: "Release"
      TARGET_FRAMEWORK: "net8.0"
      UNIT_TEST_FILTER: "TestCategory=UnitTest"
      RUN_SPECFLOW_TESTS: true
      SPECFLOW_TEST_FILTER: "TestCategory=SpecFlow"
    secrets:
      REGISTRY_USER: ${{ secrets.REGISTRY_USER }}
      REGISTRY_TOKEN: ${{ secrets.REGISTRY_TOKEN }}
```

## Input Parameters

| Parameter | Required | Default | Description |
|-----------|----------|---------|-------------|
| `SOLUTION_FILE` | ✅ | - | Path to the solution file (e.g., 'MySolution.sln') |
| `WORKING_DIRECTORY` | ❌ | `"."` | Working directory for the build |
| `DOTNET_VERSION` | ❌ | `"8.x"` | .NET version to use (6.x, 7.x, 8.x) |
| `BUILD_CONFIGURATION` | ❌ | `"Release"` | Build configuration (Debug/Release) |
| `TARGET_FRAMEWORK` | ❌ | `"net8.0"` | Target framework (net6.0, net7.0, net8.0) |
| `UNIT_TEST_FILTER` | ❌ | `"TestCategory=UnitTest"` | Filter for unit tests |
| `RUN_SPECFLOW_TESTS` | ❌ | `false` | Whether to run SpecFlow tests |
| `SPECFLOW_TEST_FILTER` | ❌ | `"TestCategory=SpecFlow"` | Filter for SpecFlow tests |

## Required Secrets

The template requires the following secrets to be configured in your repository:

| Secret | Description |
|--------|-------------|
| `REGISTRY_USER` | GitHub username for NuGet registry access |
| `REGISTRY_TOKEN` | GitHub personal access token with package read permissions |

### Setting up Secrets

1. Go to your repository **Settings** → **Secrets and variables** → **Actions**
2. Add the following repository secrets:
   - `REGISTRY_USER`: Your GitHub username
   - `REGISTRY_TOKEN`: A personal access token with `read:packages` permission

## Workflow Jobs

The template consists of four main jobs:

### 1. Build Job
- Restores NuGet packages
- Builds the solution
- Validates compilation
- Sets up subsequent test jobs

### 2. Unit Tests Job
- Automatically discovers test projects (`*Test*.csproj`)
- Creates a separate test solution
- Runs unit tests with specified filter
- Generates TRX and HTML reports
- Collects code coverage

### 3. SpecFlow Tests Job (Optional)
- Runs when `RUN_SPECFLOW_TESTS` is `true`
- Executes SpecFlow/BDD tests
- Generates detailed test reports
- Only runs after successful unit tests

### 4. Test Summary Job
- Aggregates results from all jobs
- Posts comprehensive status to PRs
- Creates GitHub step summary
- Always runs regardless of test outcomes

## Test Project Discovery

The template automatically discovers test projects using these patterns:
- **Unit Tests**: `*Test*.csproj` (excluding `*Spec*` and `*Library*`)
- **SpecFlow Tests**: Uses the main solution file with SpecFlow filter

## Test Filters

### Default Filters
- **Unit Tests**: `TestCategory=UnitTest`
- **SpecFlow Tests**: `TestCategory=SpecFlow`

### Custom Test Categories

Decorate your test methods with categories:

```csharp
[Test]
[Category("UnitTest")]
public void MyUnitTest()
{
    // Unit test code
}

[Test]
[Category("SpecFlow")]
public void MySpecFlowTest()
{
    // SpecFlow test code
}

[Test]
[Category("Integration")]
public void MyIntegrationTest()
{
    // Integration test code
}
```

## Pull Request Integration

When run on pull requests, the template provides:

- ✅ **Build Status** comments
- ✅ **Test Results** summary
- ✅ **Failure Notifications** with links to details
- ✅ **Success Confirmation** when all tests pass

Example PR comment:
```
## .NET Build and Test Summary

| Job | Status |
|-----|--------|
| **Build** | success |
| **Unit Tests** | success |
| **SpecFlow Tests** | success |

**Solution:** `MySolution.sln`
**Configuration:** `Release`
**DotNet Version:** `8.x`

✅ **All tests passed successfully!**
```

## Artifacts

The following artifacts are automatically uploaded:

- **Unit Test Results** (30-day retention)
  - TRX files for test integration
  - HTML reports for viewing
  - Code coverage data

- **SpecFlow Test Results** (30-day retention)
  - TRX files for test integration
  - HTML reports for viewing

## Troubleshooting

### Common Issues

**Build Fails - NuGet Authentication**
```
Solution: Ensure REGISTRY_USER and REGISTRY_TOKEN secrets are properly configured
```

**No Tests Discovered**
```
Solution: Verify test projects follow naming convention (*Test*.csproj)
```

**Test Filter No Matches**
```
Solution: Check test methods have correct [Category] attributes
```

**Wrong .NET Version**
```
Solution: Verify DOTNET_VERSION and TARGET_FRAMEWORK are compatible
```

### Debug Steps

1. Check the **Actions** tab for detailed logs
2. Review **test discovery** output in unit test job
3. Verify **solution file path** is correct
4. Confirm **working directory** setting
5. Check **NuGet authentication** logs

## Examples

### Multi-Framework Testing

```yaml
strategy:
  matrix:
    dotnet-version: ['6.x', '8.x']
    
jobs:
  test-matrix:
    strategy:
      matrix:
        dotnet-version: ['6.x', '8.x']
    uses: ./.github/workflows/dotnet-build-test-template.yml
    with:
      SOLUTION_FILE: "MySolution.sln"
      DOTNET_VERSION: ${{ matrix.dotnet-version }}
      TARGET_FRAMEWORK: ${{ matrix.dotnet-version == '6.x' && 'net6.0' || 'net8.0' }}
    secrets:
      REGISTRY_USER: ${{ secrets.REGISTRY_USER }}
      REGISTRY_TOKEN: ${{ secrets.REGISTRY_TOKEN }}
```

### Conditional SpecFlow Testing

```yaml
jobs:
  build-test:
    uses: ./.github/workflows/dotnet-build-test-template.yml
    with:
      SOLUTION_FILE: "MySolution.sln"
      RUN_SPECFLOW_TESTS: ${{ github.event_name == 'pull_request' }}
    secrets:
      REGISTRY_USER: ${{ secrets.REGISTRY_USER }}
      REGISTRY_TOKEN: ${{ secrets.REGISTRY_TOKEN }}
```

## File Location

Save this template as: `.github/workflows/dotnet-build-test-template.yml`

---

*This template is designed to be a comprehensive solution for .NET projects requiring automated build and test workflows with detailed reporting and PR integration.*