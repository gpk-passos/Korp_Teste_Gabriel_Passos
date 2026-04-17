using Korp.Faturamento.API.Data;
using Korp.Faturamento.API.Data.Enums;
using Korp.Faturamento.API.Models;
using Korp.Faturamento.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Korp.Faturamento.API.Repositorios
{
    public class NotaFiscalRepositorio : INotaFiscalRepositorio
    {
        private readonly AppDbContext _dbContext;
       

        public NotaFiscalRepositorio(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<List<NotaFiscalModel>> BuscarTodas()
        {
            return await _dbContext.NotasFiscais
                .Include(x => x.Itens) 
                .ToListAsync();
        }

        public async Task<NotaFiscalModel> BuscarPorId(int id)
        {
            return await _dbContext.NotasFiscais
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<NotaFiscalModel> Cadastrar(NotaFiscalModel nota)
        {
            await _dbContext.NotasFiscais.AddAsync(nota);
            await _dbContext.SaveChangesAsync(); 
            return nota;
        }

        public async Task<bool> AtualizarStatus(int id, Status status)
        {
            NotaFiscalModel nota = await BuscarPorId(id);
            if (nota == null)
            {
                return false;
            }
            nota.StatusNota = status;
            _dbContext.NotasFiscais.Update(nota);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AdicionarItem(int notaId, ItemNotaFiscalModel novoItem)
        {
            var nota = await _dbContext.NotasFiscais
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(x => x.Id == notaId);

            if (nota == null) return false;

            if (nota.StatusNota != Status.Aberta) return false;

            novoItem.NotaFiscalId = notaId; 
            nota.Itens.Add(novoItem);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Excluir(int id)
        {
            var nota = await _dbContext.NotasFiscais
                                       .Include(n => n.Itens)
                                       .FirstOrDefaultAsync(n => n.Id == id);

            if (nota == null) return false;

            _dbContext.NotasFiscais.Remove(nota);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoverItem(int notaId, int produtoId)
        {
            var nota = await _dbContext.NotasFiscais
                                       .Include(n => n.Itens)
                                       .FirstOrDefaultAsync(n => n.Id == notaId);

            if (nota == null) return false;

            var itemParaRemover = nota.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (itemParaRemover == null) return false;

            nota.Itens.Remove(itemParaRemover);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
