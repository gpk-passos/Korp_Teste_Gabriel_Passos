using System.ComponentModel.DataAnnotations;

namespace Korp.Estoque.API.Models
{
    public class ProdutoModel
    {
            public int Id { get; set; }

            public string? Codigo { get; set; }
            public string? Descricao { get; set; }

        [ConcurrencyCheck]
        public decimal Saldo { get; set; }

           
    }
}
