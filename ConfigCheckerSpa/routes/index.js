var express = require('express');
var cookieParser = require('cookie-parser');

var router = express.Router();

const dalUri = app.get('dlaUri');
const uploadEndpoint = app.get('uploadEndpoint');
const reportsEndpoint = app.get('reportsEndpoint');
const healthcheckEndpoint = app.get('healthcheckEndpoint');

/* GET home page. */
router.get('/', function (req, res, next) {
  req.headers['set-cookie'];
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
