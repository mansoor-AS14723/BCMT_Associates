using BCMT_Associates.Models;
using System.Threading.Tasks;

namespace BCMT_Associates.Interfaces
{
    public interface IUserInterface
    {
        Task<User> LoginUser(User user);
        Task<int> Add(User user);
        Task<int> Update(User user);
        Task<int> Delete(int userId, int LoginUser); 
        Task<List<User>> Get(int? userId = null, string RoleName = null);
        string ForgotPassword(User model, string origin, IConfiguration configuration, string logopath);
        Task<string> ResetPassword(User user);
       




    }
}
