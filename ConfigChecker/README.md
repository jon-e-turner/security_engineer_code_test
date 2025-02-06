# ConfigChecker -- Principal Gaming Cloud Engineer Pre-screen Code Assessment

## Technology Stack

	- ASP.Net 8.0
		- Minimal API
		- File upload service
		- Configuration validation as background service
		- Channels for communication to validation service
		- EF Core + in-memory database for report storage

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

## Configuration Notes

	- To persist the in-memory database between executions, set a value in the connection strings section
	of [appSettings.json](./appSettings.json). If not provided, the application will create a new database
	each time.