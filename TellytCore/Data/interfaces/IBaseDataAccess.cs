using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace TellytCore.Data.interfaces
{
  public interface IBaseDataAccess
  {
    public DataTable GetDataTable(string queryText, List<SqlParameter> parameters, CommandType commandType = CommandType.Text);
    public Task ExecuteNonQueryAsync(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
    public DbDataReader GetDataReader(string queryText, CommandType commandType = CommandType.Text);
    public int ExecuteNonQuery(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
  }
}
