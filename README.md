# ConfigChecker

![build status]
(https://github.com/jon-e-turner/security_engineer_code_test/actions/workflows/main_ci.yml/badge.svg)

This applcation started as a response to an interview question, and I have continued to use it to
hone my understanding of ASP.Net. There are two parts to the application at present, neither of
which is fully functional.

## ConfigChecker (the service)

This is the "back-end" of the configuration checking service. It is not a production-quality app
(again, this was an interview question) and uses a local SqlLite database file to track reports.

More in-depth setup instructions are in the app's [README](./ConfigChecker/README.md).

## ConfigCheckerSpa (the UI)

This JavaScript SPA is the front-end of the configuration checking service. It is very bare-bones
as UIs are something I've modified, but not yet built from scratch.
