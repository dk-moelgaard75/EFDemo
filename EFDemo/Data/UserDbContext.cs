using EFDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDemo.Data
{
    public class UserDbContext :DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> dbContextOptions) :base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("users");
        }
    }
}
