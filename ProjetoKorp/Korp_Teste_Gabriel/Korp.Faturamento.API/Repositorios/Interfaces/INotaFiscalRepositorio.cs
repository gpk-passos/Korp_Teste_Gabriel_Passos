using Korp.Faturamento.API.Data.Enums;
using Korp.Faturamento.API.Models;

namespace Korp.Faturamento.API.Repositorios.Interfaces
{
    public interface INotaFiscalRepositorio
    {
        Task<List<NotaFiscalModel>> BuscarTodas();
        Task<NotaFiscalModel> Cadastrar(NotaFiscalModel nota);
        Task<NotaFiscalModel> BuscarPorId(int id);
        Task<bool> AtualizarStatus(int id, Status status);
        Task<bool> AdicionarItem(int notaId, ItemNotaFiscalModel novoItem);
        Task<bool> Excluir(int notaId);
        Task<bool> RemoverItem(int notaId, int produtoId);

    }
}
