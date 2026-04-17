namespace Korp.Estoque.API.Models
{
    public class ComponenteModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; } 
        public int ProdutoId { get; set; }
    }
}
