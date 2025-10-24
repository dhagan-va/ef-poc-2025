# Testing

This project uses **xUnit** for tests and **Coverlet** for code coverage.  
You can run everything through the Makefile.

---

## Run Tests

```bash
make test
```

Runs all tests in `tests/EDI837.Ingestion.Tests` and shows results in the terminal.

---

## Run Coverage

```bash
make coverage
```

This runs tests with coverage enabled and creates an HTML report you can open in your browser.  
The report will be at:

```
tests/EDI837.Ingestion.Tests/TestResults/CoverageReport/index.html
```

---

## Run a Specific Test

You can target one test or class:

```bash
dotnet test --filter "EdiParserTests"
dotnet test --filter "TestParseEdiFileSuccess"
```
