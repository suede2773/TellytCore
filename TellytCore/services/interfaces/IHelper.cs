using TellytCore.Models;

namespace TellytCore.services.interfaces
{
  public interface IHelper
  {
    public string GenerateAssetCode(int length);
    public UserCredential GetCredential(string credentialHeader);
    public DateTime GetMountainTimeFromUTC(DateTime inputDate);
    public int GetMountainOffset(string timeZone);
  }
}
