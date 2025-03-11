# ConfigChecker

This application started as a response to an interview question, and I have continued to use it to
hone my understanding of ASP.Net. There are two parts to the application at present, neither of
which is fully functional.

This project uses [pre-commit hooks](https://pre-commit.com), so if you have an interest in
contributing, please follow the installation and initial configuration at the link above.

## Web-Queue-Worker

This application has two major parts which encapsulate the long-running task of analyzing a
configuration in the [REST API](./ConfigChecker/) service and the responsive elements in a
[JavaScript Single-Page-Application](./ConfigCheckerSpa/). As my first pass at this app, I decided
the [Web-Queue-Worker](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/web-queue-worker#web-queue-worker-on-azure-app-service)
pattern best matched the scope of the question. It is also the architecture I am most familiar
working with.

### ConfigChecker (the service)

![build status](https://github.com/jon-e-turner/security_engineer_code_test/actions/workflows/main_ci.yml/badge.svg)

The "back-end" of the configuration checking service is an Asp.NET minimal API with a local
SqlLite database file to track reports. It has basic REST API endpoints, includes a background
service to process the uploaded files, and channels are used for internal data-flow. In-depth setup
instructions are in the app's [README](./ConfigChecker/README.md#to-configure).

### ConfigCheckerSpa (the UI)

![build status](https://github.com/jon-e-turner/security_engineer_code_test/actions/workflows/spa_ci.yml/badge.svg)

This JavaScript SPA is the front-end of the configuration checking service. It includes a status
light to indicate the back-end's status, a form to upload a new configuration file for processing,
and an area to display links to the completed reports. The site is designed to work with anonymous
authentication where a user's report IDs are stored in a local, self-signed JWT (to be implemented)
brokered by the API.

## Future Plans

I intend to develop at least two more versions of this application, one using Razor Pages and the
other as a React/Node.js app.
