var isAuthenticated = false;
var userEmail = "";
var displayName = "";

$("#btnLogin").click(function () {
  $("#loadingLogin").show();
  if (ValidateLoginFields()) {
    ProcessLogin($("#loginEmail").val(), $("#loginPassword").val());
  }
  $("#loadingLogin").hide();
});

ClearLoginValidation = async () => {
  $("#loginEmail").removeClass("is-invalid");
  $("#loginPassword").removeClass("is-invalid");
  $("#loginErrorRow").hide();
}



ValidateLoginFields = () => {
  ClearLoginValidation();

  if ($("#loginEmail").val().length == 0) {
    $("#loginEmail").addClass("is-invalid");
    $("#loginErrorMessage").html("Please provide your email");
    $("#loginErrorRow").show();
    return false;
  }

  if ($("#loginPassword").val().length == 0) {    
    $("#loginPassword").addClass("is-invalid");
    $("#loginErrorMessage").html("Please provide a password");
    $("#loginErrorRow").show();
    return false;
  }
  return true;
}

ResetLoginModal = async () => {
  $("#loginEmail").val("");
  $("#loginPassword").val("");
  $("#loginErrorRow").hide();
  $("#loadingLogin").hide();
}

ProcessLogout = async () => {
  isAuthenticated = false;
  sessionStorage.removeItem("isAuthenticated");
  sessionStorage.removeItem("userEmail");
  sessionStorage.removeItem("displayName");
  CheckAuthentication();
}

ProcessLogin = async (email, password) => {
  $("#loginEmail").val("");
  $("#loginPassword").val("");

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
    $("#checkLoginReturnMessage").html(returnAuthData.message);
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
  const loginModal = bootstrap.Modal.getOrCreateInstance($('#loginModal'));
  if (!isAuthenticated) {
    let isAuthenticatedData = sessionStorage.isAuthenticated;
    if (isAuthenticatedData == undefined) {
      loginModal.show();
      $("#liLogout").hide();
    }
    else {
      displayName = sessionStorage.displayName;
      userEmail = sessionStorage.userEmail;
      isAuthenitcated = true;
      loginModal.show();
      $("#liLogout").show();
      FillAndShowGreeting(displayName);
    }
  }
  else {
    displayName = sessionStorage.displayName;
    $("#liLogout").show();
    $("#liGreet").show();
    loginModal.hide();
  }
}



