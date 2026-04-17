using Korp.Faturamento.API.Data.Enums;
using Korp.Faturamento.API.Models;
using Korp.Faturamento.API.Repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Korp.Faturamento.API.Controllers
{
    [ApiController] // 1. Avisa que é uma API profissional
    [Route("api/[controller]")] // 2. Define o caminho: api/notafiscal
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalRepositorio _repositorio;

        public NotaFiscalController(INotaFiscalRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        [HttpGet]
        public async Task<ActionResult<List<NotaFiscalModel>>> BuscarTodas()
        {
            List<NotaFiscalModel> notas = await _repositorio.BuscarTodas();
            return Ok(notas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotaFiscalModel>> BuscarPorId(int id)
        {
            NotaFiscalModel nota = await _repositorio.BuscarPorId(id);
            if (nota == null)
            {
                return NotFound($"Nota Fiscal com ID {id} não encontrada.");
            }
            return Ok(nota);
        }

        [HttpPost]
        public async Task<ActionResult<NotaFiscalModel>> Cadastrar([FromBody] NotaFiscalModel notaModel)
        {
            var todas = await _repositorio.BuscarTodas();

            int proximoNumero = (todas != null && todas.Any()) ? todas.Max(n => n.NumeroSequencial) + 1 : 1;

            notaModel.NumeroSequencial = proximoNumero;
            notaModel.StatusNota = Status.Aberta; 
            notaModel.Data = DateTime.Now;

            try
            {
                var nota = await _repositorio.Cadastrar(notaModel);
                return Ok(nota);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro no Banco: {ex.Message}");
            }
        }

        [HttpPost("imprimir/{id}")]
        public async Task<IActionResult> Imprimir(int id)
        {
            NotaFiscalModel nota = await _repositorio.BuscarPorId(id);

            if (nota == null) return NotFound("Nota não encontrada.");
            if (nota.StatusNota != Status.Aberta) return BadRequest("Apenas notas ABERTAS podem ser impressas.");

            try
            {
                using (var client = new HttpClient())
                {
                    string urlEstoque = "https://localhost:7023/api/produtos/baixar-estoque";

                    foreach (var item in nota.Itens)
                    {
                        var response = await client.PostAsJsonAsync($"{urlEstoque}/{item.ProdutoId}", item.Quantidade);

                        response.EnsureSuccessStatusCode();
                    }
                }

                await _repositorio.AtualizarStatus(id, Status.Fechada);
                return Ok(new { mensagem = "Nota impressa e estoque atualizado!", status = "Fechada" });
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Falha na integração com o estoque: {ex.Message}. A nota continua ABERTA.");
            }
        }

        [HttpPut("adicionar-item/{notaId}")]
        public async Task<ActionResult> AdicionarItem(int notaId, [FromBody] ItemNotaFiscalModel itemModel)
        {
            bool sucesso = await _repositorio.AdicionarItem(notaId, itemModel);

            if (!sucesso)
            {
                return BadRequest("Não foi possível adicionar o item. Verifique se a nota existe e se ainda está ABERTA.");
            }

            return Ok("Item adicionado com sucesso à nota fiscal!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var excluiu = await _repositorio.Excluir(id);
            if (!excluiu) return NotFound("Nota não encontrada no banco de dados.");

            return Ok(new { mensagem = "Nota removida com sucesso" });
        }

        [HttpDelete("remover-item/{notaId}/{produtoId}")]
        public async Task<ActionResult> RemoverItem(int notaId, int produtoId)
        {
            var sucesso = await _repositorio.RemoverItem(notaId, produtoId);
            if (!sucesso) return NotFound("Nota ou Item não encontrado.");

            return Ok("Item removido com sucesso.");
        }

    }
}
