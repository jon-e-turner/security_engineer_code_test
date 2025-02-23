var express = require('express');
var cookieParser = require('cookie-parser');

var router = express.Router();

const dalUri = "http://localhost:5262";
const uploadEndpoint = "upload";
const reportsEndpoint = "reports";
const healthcheckEndpoint = "healthcheck";

/* GET home page. */
router.get('/', function (req, res, next) {
  res.render('index', {
    title: 'ConfigChecker',
    reportIds: ['123', '456', '789'],
    dalUri: dalUri,
    uploadEndpoint: uploadEndpoint,
    reportsEndpoint: reportsEndpoint,
    healthcheckEndpoint: healthcheckEndpoint
  });
});

module.exports = router;
