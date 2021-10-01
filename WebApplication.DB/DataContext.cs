using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApplication.Entites;

namespace WebApplication.DB
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CurrencyHistory> CurrencyHistories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        //public async Task<int> SaveChangesAsync()
        //{
        //    return await base.SaveChangesAsync();
        //}
    }
}