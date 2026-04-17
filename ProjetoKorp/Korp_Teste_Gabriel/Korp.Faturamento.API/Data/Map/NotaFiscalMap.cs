using Korp.Faturamento.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Korp.Faturamento.API.Data.Map
{
    public class NotaFiscalMap : IEntityTypeConfiguration<NotaFiscalModel>
    {
        public void Configure(EntityTypeBuilder<NotaFiscalModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NumeroSequencial).IsRequired();

            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.StatusNota).IsRequired();

            builder.HasMany(x => x.Itens)
                   .WithOne()
                   .HasForeignKey(i => i.NotaFiscalId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
