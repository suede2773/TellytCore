using Microsoft.Data.SqlClient;
using System.Data;
using TellytCore.Data;
using TellytCore.Data.interfaces;
using TellytCore.services.interfaces;

namespace TellytCore.services
{
  public class Logging : ILogging
  {
    //private readonly IBaseDataAccess _baseDataAccess;
    private readonly IHelper _helper;

    public Logging(IHelper helper)
    {
      //_baseDataAccess = baseDataAccess;
      _helper = helper;
    }

    public async Task WriteLog(string level, string message)
    {
      //var currentMountainTime = _helper.GetMountainTimeFromUTC(TimeZoneInfo.ConvertTimeToUtc(DateTime.Now));

      //var insertParameters = new List<SqlParameter>
      //  {
      //    new SqlParameter
      //    {
      //      ParameterName = "@Level",
      //      DbType = DbType.String,
      //      Direction = ParameterDirection.Input,
      //      Value = level
      //    },
      //    new SqlParameter
      //    {
      //      ParameterName = "@Message",
      //      DbType = DbType.String,
      //      Direction = ParameterDirection.Input,
      //      Value = message
      //    },
      //    new SqlParameter
      //    {
      //      ParameterName = "@CurrentDate",
      //      DbType = DbType.DateTime,
      //      Direction = ParameterDirection.Input,
      //      Value = currentMountainTime
      //    }
      //  };

      //var writeLogQuery = @"
      //    INSERT INTO dbo.Log (Level, Message, CreatedDate)
      //    VALUES (@Level, @Message, @CreatedDate)
      //  ";

      //_baseDataAccess.ExecuteNonQuery(writeLogQuery, insertParameters, CommandType.Text);

    }

  }
}
