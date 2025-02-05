# ConfigChecker -- Principal Gaming Cloud Engineer Pre-screen Code Assessment

## Technology Stack

- ASP.Net 8.0
	- Minimal API


## Design Decisions

Minimal API allows for the quick creation of an ASP.Net back-end without much boilerplate,
and the project as defined does not require any of the features
[minimal APIs lack](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-8.0).

To ensure the application is adaptable as new requirements emerge, I will follow a simplified domain-driven design
where the data domain for this project is the ["Resource"](./Models/Resource.cs). As none of this information 
should be preserved (to preserve customer confidentiality expectations), the ["EntityBase"](./Models/EntityBase.cs)
abstract base class lacks the standard identity-tracking properties.