using System.Threading.Tasks;
using CoinTask.API.Models;

namespace CoinTask.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string email, string password);
        Task<bool> UserExists(string email);
    }
}