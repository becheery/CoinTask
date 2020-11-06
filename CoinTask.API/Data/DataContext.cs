using CoinTask.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoinTask.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<UserCoin> UserCoins { get; set; }
    }
}