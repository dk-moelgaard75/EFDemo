using System.Collections.Generic;
using System.Threading.Tasks;
using EFDemo.Models;

namespace EFDemo.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(UserModel usermodel);
        Task<bool> DeleteUser(UserModel userModel);
        Task<bool> Login(string name, string password);
        Task<List<UserModel>> ReadUsers();
        Task<UserModel> ReadUser(string name);
        Task<bool> UpdateUser(UserModel userModel);
        string LastError { get; set; }
    }
}