# ConfigCheckerSpa - The UI Layer

## Technology Stack

Built in Javascript using [Node.js](https://nodejs.org/en) and [ExpressJS](https://expressjs.com) frameworks. Static documents are templated in [PUG](https://pugjs.org) as it's the default for Express.

## Design Decisions

Ideally, the Node/Express app would handle validation before submitting the file on the user's behalf. In consideration of the import of this part of the project as a whole, I am opting to instead implement the communication with the back-end in client-side Javascript.

## To-Do

- Add indicator that DAL is online and connected
- Configure CORS and anti-forgery
- Put user's report ids into a signed JWT
  - This provides pseudo-authentication so we can experiment with that in the API
  - Obviously, not very secure. Cookie can't be modified, but it can be stolen/duplicated.
- Implement report view.

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

- Launch the application, then use a browser to open the interface. Be default, that is [localhost:3000](http://localhost:3000/).

    ```shell
    > cd security_engineer_code_test/ConfigCheckerSpa
    > npm start
    configcheckerspa@0.0.0 start
    node ./bin/www

    GET / 200 1569.806 ms - 971
    GET /stylesheets/style.css 200 10.576 ms - 111
    GET /scripts/FormHelpers.js 200 15.087 ms - 1025
    ```

- For the application to be able to submit files or read reports, you will also need to start the [ConfigChecker](../ConfigChecker/README.md#to-confgure) service.
