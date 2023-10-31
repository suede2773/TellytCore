using TellytCore.services.interfaces;

namespace TellytCore.services
{
  public class Logging : ILogging
  {
    private readonly string _logFileLocation;
    private readonly IHelper _helper;

    public Logging(string logFileLocation, IHelper helper)
    {
      _logFileLocation = logFileLocation;
      _helper = helper;
    }

    public async Task WriteLog(string level, string message)
    {
      var currentMountainTime = _helper.GetMountainTimeFromUTC(TimeZoneInfo.ConvertTimeToUtc(DateTime.Now));
      var logFileName = "TellytLog." + currentMountainTime.ToString("yyyy-MM-dd") + ".log";
      if (!File.Exists(_logFileLocation + logFileName))
      {
        File.Create(_logFileLocation + logFileName).Close();
      }

      using (var sw = File.AppendText(_logFileLocation + logFileName))
      {
        Log(level, message, sw);
      }
    }

    public async void Log(string level, string logMessage, TextWriter txtWriter)
    {
      var currentMountainTime = _helper.GetMountainTimeFromUTC(TimeZoneInfo.ConvertTimeToUtc(DateTime.Now));
      txtWriter.Write("\r\n {0} {1} :  ", currentMountainTime.ToLongTimeString(),
          currentMountainTime.ToLongDateString());
      txtWriter.WriteLine(level);
      txtWriter.WriteLine("  :{0}", logMessage);
      txtWriter.WriteLine("-------------------------------");
    }

  }
}
