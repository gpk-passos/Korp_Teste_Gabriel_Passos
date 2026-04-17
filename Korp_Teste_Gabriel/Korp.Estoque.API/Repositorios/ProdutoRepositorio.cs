using Korp.Estoque.API.Models;
using Korp.Estoque.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Korp.Estoque.API.Data;

namespace Korp.Estoque.API.Repositorios
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly AppDbContext _dbContext;

        public ProdutoRepositorio(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<ProdutoModel> BuscarPorId(int id)
        {
            // Removido o .Include(p => p.Componentes)
            return await _dbContext.Produtos
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ProdutoModel>> BuscarTodosProdutos()
        {
            return await _dbContext.Produtos
                .ToListAsync();
        }

        public async Task<ProdutoModel> Adicionar(ProdutoModel produto)
        {
            await _dbContext.Produtos.AddAsync(produto);
            await _dbContext.SaveChangesAsync();

            return produto;
        }

        public async Task<ProdutoModel> Atualizar(ProdutoModel produto, int id)
        {
            ProdutoModel produtoPorId = await BuscarPorId(id);
            if (produtoPorId == null)
            {
                return null;
            }

            produtoPorId.Codigo = produto.Codigo;
            produtoPorId.Descricao = produto.Descricao;
            produtoPorId.Saldo = produto.Saldo;

            _dbContext.Produtos.Update(produtoPorId);
            await _dbContext.SaveChangesAsync();

            return produtoPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            ProdutoModel produtoPorId = await BuscarPorId(id);
            if (produtoPorId == null)
            {
                return false;
            }

            _dbContext.Produtos.Remove(produtoPorId);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SubtrairEstoque(int id, decimal quantidade)
        {
            ProdutoModel produto = await BuscarPorId(id);
            if (produto == null || produto.Saldo < quantidade)
            {
                return false;
            }

            produto.Saldo -= quantidade;

            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}