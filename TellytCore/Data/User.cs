using TellytCore.Data.interfaces;
using TellytCore.services.interfaces;
using TellytCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace TellytCore.Data
{
  public class User : IUser
  {
    private readonly IBaseDataAccess _baseDataAccess;
    private readonly IHelper _helper;
    private readonly ILogging _logger;
    public User(IBaseDataAccess baseDataAccess, IHelper helper, ILogging logger)
    {
      _baseDataAccess = baseDataAccess;
      _helper = helper;
      _logger = logger;
    }

    public async Task<UserAccount> CreateUser(UserAccount user)
    {
      var method = "User.CreateUser";
      try
      {
        var currentMountainTime = _helper.GetMountainTimeFromUTC(TimeZoneInfo.ConvertTimeToUtc(DateTime.Now));

        var newUid = Guid.NewGuid();

        var insertParameters = new List<SqlParameter>
        {
          new SqlParameter
          {
            ParameterName = "@Id",
            DbType = DbType.Guid,
            Direction = ParameterDirection.Input,
            Value = newUid.ToString()
          },
          new SqlParameter
          {
            ParameterName = "@Email",
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            Value = user.Email
          },
          new SqlParameter
          {
            ParameterName = "@FirstName",
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            Value = user.FirstName
          },
          new SqlParameter
          {
            ParameterName = "@LastName",
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            Value = user.LastName
          },
          new SqlParameter
          {
            ParameterName = "@DisplayName",
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            Value = user.DisplayName
          },
          new SqlParameter
          {
            ParameterName = "@Password",
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            Value = user.Password
          },
          new SqlParameter
          {
            ParameterName = "@CurrentTime",
            DbType = DbType.DateTime,
            Direction = ParameterDirection.Input,
            Value = currentMountainTime
          }
        };

        var createUserQuery = @"
          INSERT INTO dbo.User (Id, Email, FirstName, LastName, DisplayName, CreatedDate, LastModifiedDate, IsPrimary, [Password])
          VALUES (@Id, @Email, @FirstName, @LastName, @DisplayName, @CurrentTime, @CurrentTime, 1, @Password)
        ";

        _baseDataAccess.ExecuteNonQuery(createUserQuery, insertParameters, CommandType.Text);
        user.IsValid = true;
        return user;
      }
      catch (Exception ex)
      {
        _logger.WriteLog("error", "Error in Method " + method);
        _logger.WriteLog("error", ex.Message);
        user.Error = ex.Message;
        user.IsValid = false;
        return user;
      }
    }

    public async Task<UserAccount> GetUser(string email)
    {
      var method = "User.GetUser";
      var userAccount = new UserAccount();

      try
      {

        var selectParameters = new List<SqlParameter>
        {
          new SqlParameter
          {
            ParameterName = "@Email",
            DbType = DbType.String,
            Direction = ParameterDirection.Input,
            Value = email
          }
      };

        var userQuery = @"
          SELECT *
          FROM dbo.[User]
          WHERE Email = @Email
        ";

        var userTable = _baseDataAccess.GetDataTable(userQuery, selectParameters);

        if (userTable.Rows.Count == 0)
        {
          return userAccount;
        }

        userAccount.FirstName = userTable.Rows[0]["FirstName"].ToString();
        userAccount.LastName = userTable.Rows[0]["LastName"].ToString();
        userAccount.Email = email;
        userAccount.Password = userTable.Rows[0]["Password"].ToString();
        userAccount.DisplayName = userTable.Rows[0]["DisplayName"].ToString();

        return userAccount;

      }
      catch(Exception ex)
      {
        _logger.WriteLog("error", "Error in Method " + method);
        _logger.WriteLog("error", ex.Message);
        return userAccount;
      }
    }
  }
}
