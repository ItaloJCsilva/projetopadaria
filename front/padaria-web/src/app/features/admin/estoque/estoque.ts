// src/app/features/admin/estoque/estoque.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Produto } from '../../../core/models/Produto';
import { ProdutoService } from '../../../core/services/ProdutoService';


@Component({
  selector: 'app-estoque',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './estoque.html'
})
export class Estoque implements OnInit {
  produtos: Produto[] = [];
  carregando = false;
  erro = '';
  sucesso = '';

  // Controle de edição: armazena o id do produto que está sendo editado
  editandoId: string | null = null;

  // Cópia dos valores para edição
  precoEdit: number = 0;
  estoqueEdit: number = 0;

  constructor(private produtoService: ProdutoService) {}

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos(): void {
    this.carregando = true;
    this.produtoService.listarTodos().subscribe({
      next: (dados: Produto[]) => {
        this.produtos = dados;
        this.carregando = false;
      },
      error: (err: any) => {
        this.erro = 'Erro ao carregar produtos.';
        this.carregando = false;
      }
    });
  }

  // Inicia a edição de um produto
  iniciarEdicao(produto: Produto): void {
    this.editandoId = produto.id;
    this.precoEdit = produto.preco;
    this.estoqueEdit = produto.estoque;
    this.erro = '';
    this.sucesso = '';
  }

  // Cancela a edição
  cancelarEdicao(): void {
    this.editandoId = null;
  }

  // Salva as alterações (preço e estoque)
  salvarEdicao(produto: Produto): void {
    // Validações básicas
    if (this.precoEdit <= 0) {
      this.erro = 'Preço deve ser maior que zero.';
      return;
    }
    if (this.estoqueEdit < 0) {
      this.erro = 'Estoque não pode ser negativo.';
      return;
    }

    this.carregando = true;
    this.erro = '';
    this.sucesso = '';

    // Monta o objeto de atualização
    const dadosAtualizados = {
      nome: produto.nome,
      descricao: produto.descricao,
      preco: this.precoEdit,
      estoque: this.estoqueEdit,
      urlImagem: produto.urlImagem,
      disponivel: produto.disponivel,
      categoriaId: produto.categoriaId
    };

    this.produtoService.atualizar(produto.id, dadosAtualizados).subscribe({
      next: (produtoAtualizado: Produto) => {
        // Atualiza a lista local
        const index = this.produtos.findIndex(p => p.id === produto.id);
        if (index !== -1) {
          this.produtos[index] = produtoAtualizado;
        }
        this.sucesso = `Produto "${produtoAtualizado.nome}" atualizado!`;
        this.cancelarEdicao();
        this.carregando = false;
      },
      error: (err: any) => {
        this.erro = err.error?.mensagem || 'Erro ao atualizar produto.';
        this.carregando = false;
      }
    });
  }

  // Apenas para exibir o status de disponibilidade
  getStatusLabel(disponivel: boolean): string {
    return disponivel ? 'Ativo' : 'Inativo';
  }
}