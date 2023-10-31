namespace TellytCore.services.interfaces
{
  public interface ILogging
  {
    public Task WriteLog(string level, string message);
  }
}
