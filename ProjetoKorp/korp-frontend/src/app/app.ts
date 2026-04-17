import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { EstoqueService } from './services/estoque';
import { FaturamentoService } from './services/faturamento';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
  standalone: false
})
export class App implements OnInit {
  produtos: any[] = [];
  notas: any[] = [];
  produtoEmEdicao: any = null;
  notaSelecionada: any = null;
  processandoId: number | null = null;
  carrinho: { ProdutoId: number, Descricao: string, Quantidade: number }[] = [];

  constructor(
    private estoqueService: EstoqueService,
    private faturamentoService: FaturamentoService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() { this.carregarTudo(); }

  carregarTudo() {
    this.estoqueService.getProdutos().subscribe({
      next: (res) => { this.produtos = res; this.cdr.detectChanges(); }
    });
    this.faturamentoService.listar().subscribe({
      next: (res) => {
        this.notas = res;
        if (this.notaSelecionada) {
          this.notaSelecionada = this.notas.find(n => n.id === this.notaSelecionada.id);
        }
        this.cdr.detectChanges();
      }
    });
  }

  getDescricaoProduto(id: any): string {
    const idNum = Number(id);
    const produto = this.produtos.find(p => Number(p.id) === idNum);
    return produto ? (produto.descricao || produto.Descricao) : `ID ${id}`;
  }

  salvarProduto(codigo: string, descricao: string, saldo: any) {
    if (!codigo || !descricao) return alert("Preencha os campos obrigatórios!");
    const p = { id: this.produtoEmEdicao?.id || 0, codigo, descricao, saldo: parseFloat(saldo) };

    if (this.produtoEmEdicao) {
      this.estoqueService.atualizar(p.id, p).subscribe({
        next: () => {
          alert("Produto atualizado com sucesso!");
          this.limparEdicao();
          this.carregarTudo();
        },
        error: (err) => {
          console.error(err);
          alert("Erro de conexão: A API de Estoque parece estar desligada ou inacessível.");
        }
      });
    } else {
      this.estoqueService.cadastrar(p).subscribe({
        next: () => {
          alert("Produto cadastrado com sucesso!");
          this.carregarTudo();
        },
        error: (err) => {
          console.error(err);
          alert("Erro de conexão: A API de Estoque parece estar desligada ou inacessível.");
        }
      });
    }
  }

  adicionarAoCarrinho(prodIdStr: string, qtdStr: string) {
    const qtd = parseFloat(qtdStr);
    const prodId = parseInt(prodIdStr);
    const produto = this.produtos.find(p => p.id === prodId);

    if (!prodIdStr || isNaN(qtd) || qtd <= 0) return alert("Quantidade inválida!");
    if (produto && produto.saldo <= 0) return alert("Produto sem saldo!");

    const itemExistente = this.carrinho.find(i => i.ProdutoId === prodId);
    if (itemExistente) { itemExistente.Quantidade += qtd; }
    else { this.carrinho.push({ ProdutoId: prodId, Descricao: produto.descricao, Quantidade: qtd }); }
  }

  injetarItemEmNota(notaId: number, prodIdStr: string, qtdStr: string) {
    const qtd = parseFloat(qtdStr);
    const prodId = parseInt(prodIdStr);
    if (!prodId || qtd <= 0) return alert("Dados inválidos.");

    const item = { produtoId: prodId, quantidade: qtd };

    // Atualização Otimista
    const notaNaTela = this.notas.find(n => n.id === notaId);
    if (notaNaTela) {
      if (!notaNaTela.itens) notaNaTela.itens = [];
      notaNaTela.itens.push(item);
    }

    this.faturamentoService.adicionarItem(notaId, item).subscribe({
      next: () => this.carregarTudo(),
      error: () => { alert("Erro ao salvar."); this.carregarTudo(); }
    });
  }

  removerItemDaNotaExistente(notaId: number, produtoId: number) {
    const notaNaTela = this.notas.find(n => n.id === notaId);
    if (notaNaTela && notaNaTela.itens) {
      const index = notaNaTela.itens.findIndex((i: any) => i.produtoId === produtoId);
      if (index !== -1) notaNaTela.itens.splice(index, 1);
    }

    this.faturamentoService.removerItemDaNota(notaId, produtoId).subscribe({
      next: () => this.carregarTudo(),
      error: () => { alert("Erro ao remover."); this.carregarTudo(); }
    });
  }

  imprimir(id: number) {
    const nota = this.notas.find(n => n.id === id);

    if (!nota || !nota.itens || nota.itens.length === 0) {
      return alert(`Atenção: Não é possível faturar a Nota #${id} pois ela está sem produtos!`);
    }

    this.processandoId = id;
    this.faturamentoService.imprimir(id).subscribe({
      next: () => {
        this.processandoId = null;
        alert(`Nota Fiscal Sequencial #${id} impressa e finalizada com sucesso!`);
        this.carregarTudo();
      },
      error: (err: any) => {
        this.processandoId = null;
        alert(err.error || `Erro ao tentar imprimir a nota #${id}.`);
        this.carregarTudo();
      }
    });
  }

  excluirNota(id: number) {
    if (confirm("Excluir nota?")) {
      this.faturamentoService.excluir(id).subscribe(() => this.carregarTudo());
    }
  }

  gerarNovaNota() {
    const nota = { statusNota: 0, itens: this.carrinho.map(i => ({ produtoId: i.ProdutoId, quantidade: i.Quantidade })) };
    this.faturamentoService.cadastrar(nota).subscribe({
      next: () => { this.carrinho = []; this.carregarTudo(); },
      error: (err: any) => alert("Erro ao gerar nota.")
    });
  }

  prepararEdicao(p: any) { this.produtoEmEdicao = p; }
  limparEdicao() { this.produtoEmEdicao = null; }
  verDetalhes(n: any) { this.notaSelecionada = this.notaSelecionada?.id === n.id ? null : n; }
  removerDoCarrinho(i: number) { this.carrinho.splice(i, 1); }
  excluir(id: number) {
    if (confirm("Deseja realmente excluir este produto?")) {
      this.estoqueService.excluir(id).subscribe({
        next: () => {
          alert("Produto removido com sucesso!");
          this.carregarTudo();
        },
        error: (err) => {
          console.error(err);
          alert("Erro de conexão: A API de Estoque parece estar desligada ou inacessível.");
        }
      });
    }
  }
}