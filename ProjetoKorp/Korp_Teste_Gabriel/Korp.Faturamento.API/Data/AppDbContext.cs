using Korp.Faturamento.API.Data.Map;
using Korp.Faturamento.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Korp.Faturamento.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }
        public DbSet<NotaFiscalModel> NotasFiscais { get; set; }
        public DbSet<ItemNotaFiscalModel> ItensNotaFiscal { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NotaFiscalMap());
            modelBuilder.ApplyConfiguration(new ItemNotaFiscalMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
