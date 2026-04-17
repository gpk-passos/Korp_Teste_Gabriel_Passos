using Korp.Faturamento.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Korp.Faturamento.API.Data.Map
{
    public class ItemNotaFiscalMap : IEntityTypeConfiguration<ItemNotaFiscalModel>
    {
        public void Configure(EntityTypeBuilder<ItemNotaFiscalModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProdutoId).IsRequired();
            builder.Property(x => x.Quantidade).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.NotaFiscalId).IsRequired();
        }
    }
}
