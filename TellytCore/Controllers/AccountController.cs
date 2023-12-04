using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using TellytCore.Models;
using TellytCore.Data.interfaces;
using System.Collections.Generic;
using System.IO;
using TellytCore.services.interfaces;

namespace TellytCore.Controllers
{

  [AllowAnonymous]
  public class AccountController : Controller
  {
    private readonly IUser _user;
    public AccountController(IUser user)
    {
      _user = user;
    }

    [HttpPost]
    public async Task<UserAccount> CreateUserAccount(string firstName, string lastName, string email, string password)
    {
      var inputUser = new UserAccount
      {
        AccountKey = string.Empty, //Add support for existing accounts later on
        City = string.Empty,
        CompanyName = string.Empty,
        Country = string.Empty,
        DisplayName = firstName + lastName,
        Email = email,
        FirstName = firstName,
        LastName = lastName,
        PostalCode = string.Empty,
        State = string.Empty,
        StreetAddress = string.Empty,
        Password = password,
      };

      return await _user.CreateUser(inputUser);
    }

    [HttpPost]
    public async Task<UserLoginResponse> Authenticate(string data)
    {
      var response = new UserLoginResponse
      {
        Message = "Invalid username and password.",
        Success = false
      };

      var request = JsonSerializer.Deserialize<UserRequest>(data);

      var userAccount = await _user.GetUser(request.Email);

      if(string.IsNullOrEmpty(userAccount.Password))
      {
        return response;
      }

      if(userAccount.Password != request.Password)
      {
        return response;
      }

      response.Success = true;
      response.DisplayName = userAccount.DisplayName;
      response.Email = userAccount.Email;


      return response;
    }

    [HttpPost]
    public async Task SendBetaEmailConfirmation(string firstName, string lastName, string email)
    {
      try
      {
        ServicePointManager.SecurityProtocol =
          SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        var betaSiteUrl = "<a href=\"https://tellytdev.azurewebsites.net/interview\">https://tellytdev.azurewebsites.net/interview</a>";

        var htmlContent = "<h2>Thanks for Helping!</h2><br>";
        htmlContent += "<p>";
        htmlContent += "Your user account has been created! You may now access the beta site at the following location: ";
        htmlContent += betaSiteUrl;
        htmlContent += "</p>";

        var smtpAddress = "smtp.gmail.com";
        var portNumber = 587;

        using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
        {
          mail.From = new MailAddress("tellytservices@gmail.com");
          mail.To.Add(email);
          mail.Subject = "Thank you for signing up with Tellyt";
          mail.Body = htmlContent;
          mail.IsBodyHtml = true;

          using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
          {
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("tellytservices@gmail.com", "rlvxxlpcxgzbyhir");
            smtp.EnableSsl = true;

            smtp.Send(mail);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

  }
}
