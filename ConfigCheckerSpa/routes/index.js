var express = require('express');
var cookieParser = require('cookie-parser');

var router = express.Router();

const guid = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;

/* GET home page. */
router.get('/', function (req, res) {
  const app = req.app;

  const dalUri = app.get('dlaUri');
  const reportsEndpoint = app.get('reportsEndpoint');
  const healthcheckEndpoint = app.get('healthcheckEndpoint');
  const cookieSecret = app.get('cookieSecret');
  let reportId = '';

  router.use(cookieParser(cookieSecret));

  if (req.params && req.params.id) {
    // Untrusted user input.
    const id = req.params.id;

    // Only valid input is a GUID, so we use a regular expression to test it.
    if (guid.test(id)) {
      reportId = `New report ID: ${id}`;
    }
  }

  res.render('index', {
    title: 'ConfigChecker',
    reportIds: ['123', '456', '789'],
    dalUri: dalUri,
    reportId: reportId,
    reportsEndpoint: reportsEndpoint,
    healthcheckEndpoint: healthcheckEndpoint
  });
});

module.exports = router;
