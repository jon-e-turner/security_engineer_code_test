const { csrfSync } = require('csrf-sync');

const { csrfSynchronisedProtection } = csrfSync({
  getTokenFromRequest: (req) => {
    if (req.is('multipart')) {
      return req.body['CsrfToken'];
    }

    return req.headers['x-csrf-token'];
  }
});

module.exports = csrfSynchronisedProtection;
