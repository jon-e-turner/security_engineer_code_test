var express = require('express');
var cookieParser = require('cookie-parser');
const app = require('../app');
const createError = require('http-errors');

var router = express.Router();

const dalUri = app.get('dlaUri');
const uploadEndpoint = app.get('uploadEndpoint');
const cookieSecret = app.get('cookieSecret');

router.use(cookieParser(cookieSecret));

function ValidateFormData(formData) {
  var isValid = false;

  if (formData && formData.has("file")) {
    var file = formData.get("file");

    isValid = file && file.size > 0
  }

  return isValid;
}

async function SendFormData(formData, resultElement, dalUri, uploadEndpoint) {
  try {
    const response = await fetch(`${dalUri}/${uploadEndpoint}`, {
      method: 'POST',
      body: formData
    });

    if (response.ok) {
      const reportId = await response.text();

      if (reportId.length > 0) {
        resultElement.value = `New report id: ${reportId}`;
      }
      else {
        console.log(await response.json());

        resultElement.value = "Whoops! Shouldn't be here."
      }
    }
    else {
      console.log(await response.json());
      console.log(`dalUri: ${dalUri}`);
      console.log(`uploadEndpoint: ${uploadEndpoint}`);

      resultElement.value = 'Error in submission.';
    }
  } catch (error) {
    console.error('Error:', error);
  }
}

router.route('/upload')
  .get(function (req, res, next) {
    // display the form
  })
  .post(async function (req, res, next) {
    // Extract the file from the request, do a first-pass validation,
    // then forward it on to the DAL for processing.
    if (!req.is(['multipart/form-data', 'application/x-www-form-urlencoded']))
      return next(createError(406));

    const formData = req.body();
  });

module.exports = router;
