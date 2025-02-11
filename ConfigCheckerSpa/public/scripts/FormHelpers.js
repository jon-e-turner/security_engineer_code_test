async function ValidateFormData(formElement) {
  var resultElement = formElement.elements.namedItem("result");
  const formData = new FormData(formElement);
  var isValid = false;

  if (formData.has("file")) {
    var file = formData.get("file");

    isValid = (typeof file === typeof File) && file.size > 0
  }

  return isValid;
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

// Override form submission
const form = document.querySelector("#uploadForm");
form.addEventListener("submit", (event) => {
  event.preventDefault();

  if (ValidateFormData(form) && dalUri && uploadEndpoint) {
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
    console.log(await response.json());
    console.log(`dalUri: ${dalUri}`);
    console.log(`uploadEndpoint: ${uploadEndpoint}`);

    resultElement.value = 'Error in form validation.';
  }
}
});
