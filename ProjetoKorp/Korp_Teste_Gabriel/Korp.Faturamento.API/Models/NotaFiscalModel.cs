

using Korp.Faturamento.API.Data.Enums;

namespace Korp.Faturamento.API.Models
{
    public class NotaFiscalModel
    {
        public int Id { get; set; }
        public int NumeroSequencial { get; set; }
        public DateTime Data {  get; set; }
       
        public Status StatusNota { get; set; }

        public List<ItemNotaFiscalModel> Itens { get; set; } = new List<ItemNotaFiscalModel>();
    }
}
