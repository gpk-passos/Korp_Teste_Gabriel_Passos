using Korp.Estoque.API.Models;
using Korp.Estoque.API.Repositorios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Korp.Estoque.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly IProdutoRepositorio _repositorio;

        public ProdutosController(IProdutoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProdutoModel>>> BuscarTodosProdutosAsync()
        {
               List<ProdutoModel> produtos = await _repositorio.BuscarTodosProdutos();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoModel>> BuscarPorId(int id)
        {
            ProdutoModel produto = await _repositorio.BuscarPorId(id);
            if (produto == null)
            {
                return NotFound($"Produto {id} não encontrado.");
            }
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoModel>> Cadastrar([FromBody] ProdutoModel produtoModel)
        {
           ProdutoModel produto = await _repositorio.Adicionar(produtoModel);

            return Ok(produto);
        }

        [HttpPost("baixar-estoque/{id}")]
        public async Task<IActionResult> BaixarEstoque(int id, [FromBody] decimal quantidade)
        {
            bool sucesso = await _repositorio.SubtrairEstoque(id, quantidade);
            if (!sucesso)
            {
                return BadRequest("Saldo insuficiente ou produto inexistente.");
            }
                return Ok("Estoque atualizado com sucesso!");
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoModel>> Atualizar([FromBody] ProdutoModel produtoModel, int id)
        {
            produtoModel.Id = id;
            ProdutoModel produto = await _repositorio.Atualizar(produtoModel, id);

            if (produto == null)
            {
                return NotFound($"Produto com ID {id} não encontrado para atualização.");
            }

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoModel>> Apagar(int id)
        {
            bool apagado = await _repositorio.Apagar(id);

            if (!apagado)
            {
                return NotFound("Produto não encontrado.");
            }

            return Ok(apagado);
        }
    }
}
