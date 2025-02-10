async function ValidateFormData(formElement, dalUri, uploadEndpoint) {
  var resultElement = formElement.elements.namedItem("result");
  const formData = new FormData(formElement);
  var isValid = false;

  if (formData.has("file")) {
    var file = formData.get("file");

    isValid = (typeof file === typeof File) && file.size > 0
  }

  if (isValid) {
    const response = await SendFormData(formData, dalUri, uploadEndpoint)

    if (response.ok) {
      const reportId = response.text

      if (reportId) {
        resultElement.value = `New report id: ${reportId}`;
      }
    }
    else {
      resultElement.value = 'Error in submission.';
    }
  }
  else {
    resultElement.value = 'Error in form validation.';
  }
}

async function SendFormData(formData, dalUri, uploadEndpoint) {
  try {
    return await fetch(`${dalUri}/${uploadEndpoint}`, {
      method: 'POST',
      body: formData
    });
  } catch (error) {
    console.error('Error:', error);
  }
}
