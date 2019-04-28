using EFDemo.Data;
using EFDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDemo.Services
{
    public class UserService : IUserService
    {
        private static ConcurrentBag<UserModel> _userStore;
        private static DbContextOptions<UserDbContext> _dbContextOptions;
        //private static UserDbContext _userDbContext;
        public string LastError { get; set; }

        

        public UserService(DbContextOptions<UserDbContext> dbContextOptions)
        {
            _userStore = new ConcurrentBag<UserModel>();
            _dbContextOptions = dbContextOptions;
            UserDbContext _userDbContext = new UserDbContext(_dbContextOptions);
            _userDbContext.Database.EnsureCreated();
        }
        public Task<bool> CreateUser(UserModel userModel)
        {
            //todo - check if username or email exists
            if (userModel.LongId == Guid.Empty)
            {
                userModel.LongId = Guid.NewGuid();
            }
            UserDbContext _userDbContext = new UserDbContext(_dbContextOptions);
            _userDbContext.Add(userModel);
            _userDbContext.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> Login(string name, string password)
        {
            UserDbContext _userDbContext = new UserDbContext(_dbContextOptions);
            UserModel curUser = _userDbContext.Users.FirstOrDefault(u => u.FirstName == name && u.Password == password);
            return Task.FromResult(curUser != null ? true : false);
        }
        public Task<UserModel> ReadUser(string name)
        {
            //TDOD - add logic to get a single user
            UserDbContext _userDbContext = new UserDbContext(_dbContextOptions);
            UserModel curUser = _userDbContext.Users.FirstOrDefault(u => u.FirstName == name);
            return Task.FromResult(curUser);
        }
        public async Task<List<UserModel>> ReadUsers()
        {
            //TDOD - add logic to get all users
            UserDbContext _userDbContext = new UserDbContext(_dbContextOptions);
            List<UserModel> list = await _userDbContext.Users.ToListAsync();
            return list;
        }
        public Task<bool> UpdateUser(UserModel userModel)
        {
            //TODO - add logic for updating user
            UserDbContext _userDbContext = new UserDbContext(_dbContextOptions);
            _userDbContext.Update(userModel);
            _userDbContext.SaveChanges();
            return Task.FromResult(true);
        }
        public Task<bool> DeleteUser(UserModel userModel)
        {
            //TODO - add logic for deleting user
            UserDbContext _userDbContext = new UserDbContext(_dbContextOptions);
            _userDbContext.Remove(userModel);
            _userDbContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
