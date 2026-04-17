using Korp.Estoque.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Korp.Estoque.API.Data.Map

{
    public class ProdutoMap : IEntityTypeConfiguration<ProdutoModel>
    {
        public void Configure(EntityTypeBuilder<ProdutoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Codigo).IsRequired().HasMaxLength(160);
            builder.Property(x => x.Descricao).IsRequired().HasMaxLength(150);

        }
    }
}
