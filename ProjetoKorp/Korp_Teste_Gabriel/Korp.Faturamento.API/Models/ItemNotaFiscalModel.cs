namespace Korp.Faturamento.API.Models
{
    public class ItemNotaFiscalModel
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; } 
        public decimal Quantidade { get; set; }
        public int NotaFiscalId { get; set; }
    }
}
