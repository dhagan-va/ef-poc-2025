# Setup
Note: This project was built on a Mac so may have subtle differences for someone running on a Windows machine.

Setup according to commands in `install-notes.md` unless you have a different
environment type in which case you may have to adapt some of the instructions.

# Run Migrations

## Step 1: Create and setup appsettings.Development.json
Copy the `src/EDI837.Ingestion/appsettings.Development.example.json` file, name it `appsettings.Development.json` in the same directory.

## Step 2: Update DB Connection information
Change attributes to match your DB connection information.

## Step 3: Run the migrations
In the `clint_smith` directory level, run
```
make create-db
```

This runs the `dotnet ef update` command on both DB Contexts in the project. See the Makefile entry for more details.

## Step 4: Verify DBs were created
Open up your database manager and make sure you see CLAIM_STAGING and HIPAA_5010_837P DBs


# Running Project

## Step 1: Ingest local sample file
This step will read a sample file from the repo that is already checked in,
parse it, and save to both DBs

```
make run-ingest-local
```

## Step 2: Verify it worked

You can open the the CLAIM_STAGING DB and make sure you have a record in the ClaimStagings table and then go to the HIPAA_5010_837P DB and look in the T837P table and verify the record is there.

## Step 3: Verify it won't add a duplicate
Run the same make command again and verify that you get an error letting you know that the duplicate transaction cannot be created.

## Step 4: Drop and Recreate DBs

```
make reset-db
```
This command should drop and recreate both databases fresh.

## Step 5: Run S3 Version

```
make run-ingest-s3
```

This will create a polling task that will look for incoming files to s3.
The task will start Moto, provided you have it installed as described in the install-notes.md file.

Once this is running, move on to next step

## Step 6: Add a file to S3
```
# In another terminal window
make populate-s3
```
This will load the sample file from the repo to the S3 url.

## Step 7: Verify processing
Switch back to the original terminal window and watch for file to be picked up and processed.

Repeat Step 2 to verify it worked.
Repeat Step 3 but with the S3 version of the command

## Step 8: Process a second file
```
make populate-s3 SAMPLE_PATH=samples/837-sample-file-2.edi
```

Repeat Step 2 for verification


# Running Tests
## Without coverage
```
make test
```

## With coverage
```
make coverage
```



