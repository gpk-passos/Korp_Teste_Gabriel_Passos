using Korp.Estoque.API.Data.Map;
using Korp.Estoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Korp.Estoque.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base (options)
        {
            
        }

        public DbSet<ProdutoModel> Produtos { get; set; }
    

        //public DbSet<CategoriasModel> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            base.OnModelCreating(modelBuilder);
        }


    }
}
