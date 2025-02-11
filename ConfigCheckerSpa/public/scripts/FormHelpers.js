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

