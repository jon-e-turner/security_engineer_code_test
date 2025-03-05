# ConfigChecker

## Technology Stack

- ASP.Net 8.0
  - Minimal API
  - REST Endpoints
    - File upload (POST)
    - Healthcheck (GET)
    - Reports (GET)
    - JWT (TBD)
  - Configuration validation as background service
  - Channels for inter-module communication
  - EF Core + SqlLite database for report storage

## Design Decisions

Minimal API allows for the quick creation of an ASP.Net back-end without much boilerplate,
and the project as defined does not require any of the features
[minimal APIs lack](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-8.0).

To ensure the application is adaptable as new requirements emerge, I will follow a simplified
domain-driven design. As we only want to persist the reports, and not the raw configuration data,
the data domain is the ["Finding"](./Models/Finding.cs). A "report" is a collection of "findings".

Simple data-transfer objects are used to represent objects in memory, and we use channels to
provide data flow between the file upload endpoint and the configuration checker. This is the
"queue" portion of the application.

The channel is read by a [background service](./Services/ConfigurationAnalyzer.cs), which reads
the uploaded file, parses its JSON into [resources](./Dtos/ResourceDto.cs), and then dispatches
tasks to asynchronously analyze them in parallel. Results are collected and passed off to the
persistence service for storage. This is the "worker" portion of the application.

Persistence in this application is handled by a local SqlLite file. If deploying such a service
into production, it is best suited to a document-based system like Azure's [CosmosDb](https://learn.microsoft.com/en-us/azure/cosmos-db/).

Entity-type relationships are configured using the IEntityTypeConfiguration interface to ensure
the domains remain clean and decoupled from the persistence implementation.

### Design Changes for Production

The `ConfigurationAnalyzer` service is designed to scale horizontally in order to handle a
variable amount of inputs, but is constrained from doing so by coupling to the REST endpoint. A
good next step would be to move the worker into its own project, allowing it to be deployed
independently of the API or web front-end. This would also likely require moving from channels
to a hosted messaging bus.

As mentioned above, this data is very well-suited to a document-based storage solution, and I
would recommend deploying it with CosmosDB or DocumentDB for production persistence. This would
also integrate well with bring-your-own-key, document-level encryption to ensure user privacy.

Ideally, this type of API would only be accessible to verified users, so it should be integrated
into a directory service for authentication. The domain model may need updated to reflect the
directory entity (user, group, etc.) which owns the record.

## To configure

- Clone the repository to your local machine.

    ``` shell
    > git clone git://github.com/jon-e-turner/security_engineer_code_test
    > cd security_engineer_code_test/ConfigChecker
    ```

- Ensure you have `dotnet-ef` tools at version 9.0.1 or higher, then run the database migration to create
 the initial file.

    ``` shell
    > dotnet tool list --global dotnet-ef
    > dotnet tool update --global dotnet-ef # If needed
    > dotnet ef database update
    ```

- Launch the application, then continue on to launching the [SPA](../ConfigCheckerSpa/README.md#to-configure).

    ``` shell
    > dotnet run
    info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5262
    info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
    info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
    ```
