using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace AuthService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userAdmin = new User
            {
              Id = 1,
              Email = "administrador@teste.com",
              Name = "Administrador",
              Role = Role.Administrador  
            };
            userAdmin.createPasswordHash("admin123456");
            var userClient = new User
            {
              Id = 2,
              Email = "client@teste.com",
              Name = "Client",
              Role = Role.Client  
            };
            userClient.createPasswordHash("client123456");
            modelBuilder.Entity<User>().HasData(
                userAdmin,
                userClient
            );
        }
    }
}