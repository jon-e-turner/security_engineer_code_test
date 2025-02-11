# ConfigChecker -- Principal Gaming Cloud Engineer Pre-screen Code Assessment

## Technology Stack

- ASP.Net 8.0
  - Minimal API
  - File upload service
  - Configuration validation as background service
  - Channels for communication to validation service
  - EF Core + Sqllite database for report storage

## Design Decisions

Minimal API allows for the quick creation of an ASP.Net back-end without much boilerplate,
and the project as defined does not require any of the features
[minimal APIs lack](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-8.0).

To ensure the application is adaptable as new requirements emerge, I will follow a simplified domain-driven design.
As we only want to persist the reports, and not the raw configuration data, the data domain is the
["Finding"](./Models/Finding.cs).

Simple data-transfer objects are used for user interactions.

Channels are used to provide data flow between the file uploader and the configuration checker. This allows
the API to respond to the user quickly and moves analyzing the configuration out of the hot-path.

Configuring entity-type relationships using the IEntityTypeConfiguration interface to ensure the domains
remain clean and decoupled from the persistence implementation.

## To confgure

- Clone the repository to your local machine

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

- Launch the application, then use a browser to open URI from the debug output. In this example,
that is `http://localhost:5262`. If you are re-directed from "/" to "/upload" the service is
connected.

    ``` shell
    > dotnet run
    info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5262
    info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
    info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
    ```
