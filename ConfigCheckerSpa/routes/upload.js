var express = require('express');
//var cookieParser = require('cookie-parser');
const createError = require('http-errors');

var router = express.Router();

//router.use(cookieParser(cookieSecret));

function ValidateFormData(formData) {
  var isValid = false;

  if (formData && formData.has("file")) {
    var file = formData.get("file");

    isValid = file && file.size > 0
  }

  return isValid;
}

async function SendFormData(formData, dalUri, uploadEndpoint) {
  try {
    const response = await fetch(`${dalUri}/${uploadEndpoint}`, {
      method: 'POST',
      body: formData
    });

    if (response.ok) {
      const reportId = await response.text();

      if (reportId.length > 0) {
        return reportId;
      }
      else {
        console.log(await response.json());
      }
    }
    else {
      console.log(await response.json());
      console.log(`dalUri: ${dalUri}`);
      console.log(`uploadEndpoint: ${uploadEndpoint}`);
    }
  } catch (error) {
    console.error('Error:', error);
  }
}

router.route('/upload')
  .get(function (req, res) {
    res.redirect('/index');
  })
  .post(async function (req, res, next) {
    const app = req.app;

    const dalUri = app.get('dalUri');
    const uploadEndpoint = app.get('uploadEndpoint');


    // Extract the file from the request, do a first-pass validation,
    // then forward it on to the DAL for processing.
    if (!req.is(['multipart/form-data', 'application/x-www-form-urlencoded']))
      return next(createError(406));

    const formData = new FormData(req.body());
    const resultElement = formData.elements.namedItem("result");

    if (ValidateFormData(formData)) {
      const response = await SendFormData(formData, resultElement, dalUri, uploadEndpoint);

      if (response && response.length > 0) {
        res.redirect(`/index?id=${response}`)
      }
    }
  });

module.exports = router;
