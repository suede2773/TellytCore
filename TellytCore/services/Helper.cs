using System.Runtime.InteropServices;
using System.Text;
using TellytCore.services.interfaces;
using TellytCore.Models;

namespace TellytCore.services
{
  public class Helper : IHelper
  {

    public Helper()
    {
    }
    public UserCredential GetCredential(string credentialHeader)
    {
      var user = new UserCredential();
      try
      {
        var credentialString = UTF8Encoding.UTF8.GetString(Convert.FromBase64String(credentialHeader.Replace("Basic ", string.Empty)));
        var separatorIndex = credentialString.IndexOf(':');
        if (separatorIndex >= 0)
        {
          user.username = credentialString.Substring(0, separatorIndex);
          user.password = credentialString.Substring(separatorIndex + 1);
        }
        return user;
      }
      catch
      {
        return user;
      }
    }

    public string GenerateAssetCode(int length)
    {
      var random = new Random();
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

      return new string(Enumerable.Repeat(chars, 8)
          .Select(s => s[random.Next(s.Length)]).ToArray());

    }

    public DateTime GetMountainTimeFromUTC(DateTime inputDate)
    {
      inputDate = inputDate.ToUniversalTime();
      var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
      TimeZoneInfo mountainTimeZone;
      if (isWindows)
      {
        mountainTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
      }
      else
      {
        mountainTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Denver");
      }
      DateTime mountainTime = TimeZoneInfo.ConvertTimeFromUtc(inputDate, mountainTimeZone);
      return mountainTime;
    }


    public int GetMountainOffset(string timeZone)
    {
      var mountainTimeOffset = 0;
      switch (timeZone)
      {
        case "PST":
        case "PDT":
          mountainTimeOffset = 1;
          break;
        case "CST":
        case "CDT":
          mountainTimeOffset = -1;
          break;
        case "EDT":
        case "EST":
          mountainTimeOffset = -2;
          break;
        case "GMT":
          mountainTimeOffset = -6;
          break;
        case "BST":
          mountainTimeOffset = -7;
          break;
      }

      return mountainTimeOffset;
    }
  }
}
