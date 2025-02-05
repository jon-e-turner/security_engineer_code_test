# ConfigChecker -- Principal Gaming Cloud Engineer Pre-screen Code Assessment

## Technology Stack

- ASP.Net 8.0
	- Minimal API
	- File upload service
	- Configuration validation service


## Design Decisions

Minimal API allows for the quick creation of an ASP.Net back-end without much boilerplate,
and the project as defined does not require any of the features
[minimal APIs lack](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-8.0).

To ensure the application is adaptable as new requirements emerge, I will follow a simplified domain-driven design.
The input data domain for this project is the ["Resource"](./Models/Resource.cs). The output data domain will is the
["Finding"](./Models/Finding.cs).