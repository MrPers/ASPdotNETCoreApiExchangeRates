using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.DB.Entites;

namespace WebApplication.DB
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CurrencyHistory> CurrencyHistories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        protected DbSet<AnswerCurrencyHistory> AnswerCurrencyHistory { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerCurrencyHistory>().HasNoKey();
        }

        public async Task<List<AnswerCurrencyHistory>> GetCurrrencyHistory(long currencyId, string scale, string dtStart, string dtFinal)
        {
            return await this.Set<AnswerCurrencyHistory>().FromSqlRaw($"EXEC GetCurrencyHistories '{scale}',{currencyId},'{dtStart}','{dtFinal}'").ToListAsync();
        }
    }
}