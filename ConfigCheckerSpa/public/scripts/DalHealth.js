function CheckDalHealth(dalUri, endpoint) {
  try {
    return fetch(`${dalUri}/${endpoint}`)
      .then(res => {
        console.log(res);
        if (res.ok) {
          return true;
        }
        return false;
      }, reason => {
        console.log(reason);
        return false;
      })
  }
  catch (error) {
    console.log(error);
    return false;
  }
}
