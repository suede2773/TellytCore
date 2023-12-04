using TellytCore.Models;

namespace TellytCore.Data.interfaces
{
  public interface IUser
  {
    public Task<UserAccount> CreateUser(UserAccount user);
    public Task<UserAccount> GetUser(string email);

  }
}
