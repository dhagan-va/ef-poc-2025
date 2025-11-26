```markdown
# EF POC 2025 – Project Setup & Developer Guide

This repository is a proof of concept for working with EDI parsing, Entity Framework Core, and mocked AWS services using Moto. This guide explains how to set up the development environment, configure dependencies, and run the solution locally.

---

## Repository

Clone the repository from:

```

[https://github.com/dhagan-va/ef-poc-2025.git](https://github.com/dhagan-va/ef-poc-2025.git)

```

---

## Project Setup

### Clone and Organize Your Work

1. Open **Visual Studio**.
2. Click **Clone a repository**.
3. Use the following repository URL:
```

[https://github.com/dhagan-va/ef-poc-2025.git](https://github.com/dhagan-va/ef-poc-2025.git)

```
4. Create a new branch for your work.
5. Inside the repository, create a folder using the format:
```

<first-name>-<last-initial>

```
Example:
```

carlos-l

```
6. Inside your folder, create:
- A new solution
- One of the following project types:
  - Console Application
  - Website
  - Web API

---

## Required NuGet Packages

Install the following packages:

### Application Packages

- **EdiFabric** – EDI parsing support  
- **EdiFabric.Templates.Hippa** – HIPAA EDI templates  
- **Microsoft.EntityFrameworkCore** – ORM framework  
- **Microsoft.EntityFrameworkCore.SqlServer** – SQL Server provider  
- **Microsoft.EntityFrameworkCore.Tools** – Migration tools  

---

## EdiFabric License Setup

A license key is required for EdiFabric.

Get your free license here:  
https://support.edifabric.com/hc/en-us/articles/360000280532-Free-code-to-master-your-EDI-files

Register the license key in your .NET Core startup pipeline.

---

## Test Project Setup

### Add a Test Project

Create a test project using one of the following frameworks:

- xUnit
- NUnit

### Test Dependencies

Install the following NuGet packages:

- **Microsoft.EntityFrameworkCore**
- **Microsoft.EntityFrameworkCore.InMemory** – In-memory database provider
- **Moq** – Mocking and dependency injection

### Test Configuration

Use:
- **Setup()** in NUnit OR
- Class constructor in xUnit

to register:
- DB context
- Configuration
- External services

Create and organize your tests logically by responsibility.

---

## AWS Mocking Setup (Moto)

### Prerequisites

Ensure that Python is installed (pip is included automatically).

---

### Install Moto

Instructions:
https://docs.getmoto.org/en/latest/docs/getting_started.html
Run:
```

pip install moto[server]

```

---

### Make Moto Available Globally (Optional)

If `moto_server` is not recognized:

Use:

```

python -m moto.server

```

To enable `moto_server` globally:

1. Search for **Environment Variables** in Windows.
2. Click **Environment Variables**.
3. Under **System Variables**, select `Path`.
4. Add your Python Scripts folder (example):
```

C:\Users<your-user>\AppData\Roaming\Python\Python314\Scripts

```
5. Restart your terminal.

---

## AWS CLI Configuration

### Install AWS CLI

Follow the official documentation:  
https://docs.aws.amazon.com/cli/v1/userguide/install-windows.html

---

### Set Environment Variables

Add the following under **User Environment Variables**:

```

AWS_ACCESS_KEY_ID=testing
AWS_SECRET_ACCESS_KEY=testing
AWS_SECURITY_TOKEN=testing
AWS_SESSION_TOKEN=testing
AWS_DEFAULT_REGION=us-east-1

```

Restart your terminal after adding variables.

---

## AWS SDK Setup (.NET)

Install:

- **AWSSDK.Core**
- **AWSSDK.Extensions.NETCore.Setup**

---

## Running the Project

At the root of the repository, you will find two startup scripts:

---

### MotoStartup.cmd

Handles:
- Terminating existing Moto server processes
- Launching a fresh Moto instance

---

### S3Startup.cmd

Handles:
- Creating the `edi-bucket`
- Uploading sample files from the `/samples` directory

---

### PowerShell Orchestration

File:
```

StartUp.ps1

```

This script:
- Executes both `.cmd` files
- Is wired into the project as a **Pre-Build Event**

### When the solution builds:

- Moto server starts
- AWS mock services become available
- S3 bucket and data are created automatically
- Application is ready for testing and execution

---

## Verification Checklist

Confirm the following before running the solution:

- ✅ Python installed  
- ✅ Moto installed  
- ✅ AWS CLI installed  
- ✅ Environment variables configured  
- ✅ Moto server running  
- ✅ `edi-bucket` exists  
- ✅ Sample files uploaded  
- ✅ Build completes successfully  

---

## Troubleshooting

### Moto not recognized
Run:

```

python -m moto.server

```

Verify Python scripts folder is in your `PATH`.

---

### AWS CLI not responding
Confirm environment variables are set and terminal was restarted.

---

### Bucket not found
Run `S3Startup.cmd` manually if needed.

---

## Maintainer Notes

- Keep credentials fake (Moto only).
- Never commit license keys or secrets.
- All AWS interactions are mock-only.
- Follow naming conventions strictly.
```

---
