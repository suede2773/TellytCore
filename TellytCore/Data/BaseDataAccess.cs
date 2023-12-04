using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using TellytCore.Data.interfaces;
using TellytCore.services.interfaces;

namespace TellytCore.Data
{
  public class BaseDataAccess : IBaseDataAccess
  {
    private readonly IConfiguration _configuration;
    public string ConnectionString { get; set; }
    public BaseDataAccess(IConfiguration config, ILogging logger)
    {
      _configuration = config;
      this.ConnectionString = _configuration.GetConnectionString("Stream");
    }

    public SqlConnection GetConnection()
    {
      SqlConnection connection = new SqlConnection(this.ConnectionString);
      if (connection.State != ConnectionState.Open)
        connection.Open();
      return connection;
    }

    public DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
    {
      SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
      command.CommandType = commandType;
      return command;
    }

    public SqlParameter GetParameter(string parameter, object value)
    {
      SqlParameter parameterObject = new SqlParameter(parameter, value != null ? value : DBNull.Value);
      parameterObject.Direction = ParameterDirection.Input;
      return parameterObject;
    }

    public SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
    {
      SqlParameter parameterObject = new SqlParameter(parameter, type); ;

      if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
      {
        parameterObject.Size = -1;
      }

      parameterObject.Direction = parameterDirection;

      if (value != null)
      {
        parameterObject.Value = value;
      }
      else
      {
        parameterObject.Value = DBNull.Value;
      }

      return parameterObject;
    }

    public async Task ExecuteNonQueryAsync(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
      try
      {
        using (SqlConnection connection = this.GetConnection())
        {
          DbCommand cmd = GetCommand(connection, procedureName, commandType);

          if (parameters != null && parameters.Count > 0)
          {
            cmd.Parameters.AddRange(parameters.ToArray());
          }

          await cmd.ExecuteNonQueryAsync();
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public int ExecuteNonQuery(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
      int returnValue = -1;

      try
      {
        using (SqlConnection connection = this.GetConnection())
        {
          DbCommand cmd = this.GetCommand(connection, procedureName, commandType);

          if (parameters != null && parameters.Count > 0)
          {
            foreach (var param in parameters)
            {
              var parameter = cmd.CreateParameter();
              parameter.ParameterName = param.ParameterName;
              parameter.Value = param.Value;
              parameter.DbType = param.DbType;
              param.Direction = param.Direction;
              cmd.Parameters.Add(parameter);
            }
          }

          returnValue = cmd.ExecuteNonQuery();
        }
      }
      catch (Exception ex)
      {
        throw;
      }

      return returnValue;
    }

    public object ExecuteScalar(string procedureName, List<SqlParameter> parameters)
    {
      object returnValue = null;

      try
      {
        using (DbConnection connection = this.GetConnection())
        {
          DbCommand cmd = this.GetCommand(connection, procedureName, CommandType.StoredProcedure);

          if (parameters != null && parameters.Count > 0)
          {
            cmd.Parameters.AddRange(parameters.ToArray());
          }

          returnValue = cmd.ExecuteScalar();
        }
      }
      catch (Exception ex)
      {
        throw;
      }

      return returnValue;
    }

    public DataTable GetDataTable(string queryText, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
    {
      var dt = new DataTable();

      using (SqlConnection connection = this.GetConnection())
      {
        using (SqlDataAdapter sda = new SqlDataAdapter(queryText, connection))
        {
          sda.SelectCommand.CommandType = CommandType.Text;
          if (parameters != null)
          {
            foreach (var parameter in parameters)
            {
              sda.SelectCommand.Parameters.Add(parameter);
            }
          }
          sda.Fill(dt);
        }
      }
      return dt;
    }

    public DbDataReader GetDataReader(string queryText, CommandType commandType = CommandType.Text)
    {
      DbDataReader ds;

      try
      {
        DbConnection connection = this.GetConnection();
        {
          DbCommand cmd = this.GetCommand(connection, queryText, commandType);
          ds = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
      }
      catch (Exception ex)
      {
        throw;
      }

      return ds;
    }

    public DbDataReader GetDataReader(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
      DbDataReader ds;

      try
      {
        DbConnection connection = this.GetConnection();
        {
          DbCommand cmd = this.GetCommand(connection, procedureName, commandType);
          if (parameters != null && parameters.Count > 0)
          {
            cmd.Parameters.AddRange(parameters.ToArray());
          }

          ds = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
      }
      catch (Exception ex)
      {
        throw;
      }

      return ds;
    }
  }
}
