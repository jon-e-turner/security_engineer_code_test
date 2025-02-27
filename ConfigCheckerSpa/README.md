# ConfigCheckerSpa - The UI Layer

## Technology Stack

Built in Javascript using [Node.js](https://nodejs.org/en) and [ExpressJS](https://expressjs.com) frameworks. Static documents are templated in [PUG](https://pugjs.org) as it's the default for Express.

## Design Decisions

Many of my decisions here are based on how I remember the main web application one of my previous teams maintained. This implementation needs re-worked from scratch.

The Express app handles authenticating the user or establishing a new session based on the user's JWT. It also creates the static web content from Pug templates, filling in configuration values where appropriate. Ideally, the Express app would also validate the uploaded file before submitting it to the API.

I opted instead to implement the communication with the API in client-side JavaScript, which is causing quite a few headaches. Client-side scripts also handle validating the status of the API through the healthcheck endpoint.

## To-Do

- Move file validation and upload into the Express app
  - Aim for minimal client-side scripting.
- Put user's report ids into a signed JWT
  - This provides pseudo-authentication so we can experiment with that in the API
  - Obviously, not very secure. Cookie can't be modified, but it can be stolen/duplicated.
- Implement report view.
- Update the app's style.

## To configure

- Clone the repository to your local machine.

    ``` shell
    > git clone git://github.com/jon-e-turner/security_engineer_code_test 
    > cd security_engineer_code_test/ConfigCheckerSpa
    ```

- Ensure you have Node.js and npm installed and up-to-date.

    ``` shell
    > node -v
    v22.13.1
    > npm -v
    11.1.0
    ```

- Launch the application, then use a browser to open the interface. By default, that is [localhost:3000](http://localhost:3000/).

    ```shell
    > cd security_engineer_code_test/ConfigCheckerSpa
    > npm start
    configcheckerspa@0.0.0 start
    node ./bin/www

    GET / 200 1569.806 ms - 971
    GET /stylesheets/style.css 200 10.576 ms - 111
    GET /scripts/FormHelpers.js 200 15.087 ms - 1025
    ```

- For the application to be able to submit files or read reports, you will also need to start the [ConfigChecker](../ConfigChecker/README.md#to-configure) service.
