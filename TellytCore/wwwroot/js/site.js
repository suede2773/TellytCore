var isAuthenticated = false;
var userEmail = "";
var displayName = "";

ProcessLogout = async () => {
  isAuthenticated = false;
  sessionStorage.removeItem("isAuthenticated");
  sessionStorage.removeItem("userEmail");
  sessionStorage.removeItem("displayName");
  //$("#liGreet").html("");
  //$("#liGreet").hide();
  CheckAuthentication();
}

ProcessLogin = async (email, password) => {
  $("#inputEmail").val("");
  $("#inputPassword").val("");

  let formData = new FormData();
  const authenticateInfo = {
    "email": email,
    "password": password
  };

  formData.append("data", JSON.stringify(authenticateInfo));

  const authUrl = window.location.protocol + '//' + window.location.host + "/Account/Authenticate";
  const rawAuthResponse = await fetch(authUrl, {
    referrerPolicy: 'origin',
    method: 'POST',
    body: formData
  });

  const returnAuthData = await rawAuthResponse.json();

  $("#loadingSendUserInfo").hide();

  if (returnAuthData.success) {
    sessionStorage.setItem("isAuthenticated", "true");
    sessionStorage.setItem("userEmail", returnAuthData.email);
    sessionStorage.setItem("displayName", returnAuthData.displayName);
    FillAndShowGreeting(returnAuthData.displayName);
    isAuthenticated = true;
    CheckAuthentication();
  }
  else {
    $("#checkModalReturnMessage").html(returnAuthData.message);
  }
}

FillAndShowGreeting = async (dislayName) => {
  let greetHtml = "<span>";
  greetHtml += "Welcome ";
  greetHtml += dislayName;
  greetHtml += "!</span>";

  $("#liGreet").html(greetHtml);
  $("#liGreet").show();
}

CheckAuthentication = async () => {
  //$("#divEvents").hide();
  if (!isAuthenticated) {
    let isAuthenticatedData = sessionStorage.isAuthenticated;
    if (isAuthenticatedData == undefined) {
      $("#divAuthenticate").show();
      $("#liLogout").hide();
    }
    else {
      displayName = sessionStorage.displayName;
      userEmail = sessionStorage.userEmail;
      isAuthenitcated = true;
      $("#divAuthenticate").hide();
      $("#liLogout").show();
      FillAndShowGreeting(displayName);
      //$("#divEvents").show();
      //GetEventData();
    }
  }
  else {
    displayName = sessionStorage.displayName;
    $("#liLogout").show();
    $("#liGreet").show();
    $("#divAuthenticate").hide();

    //$("#divEvents").show();
    //GetEventData();
  }
}



