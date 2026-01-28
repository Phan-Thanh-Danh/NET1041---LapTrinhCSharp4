using Bai2.Models;

namespace Bai2.Services
{
    public interface IUserService
    {
        List<User> GetUsers();
        void AddUser(User user);
        User? GetUserById(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
