import { Component, OnInit } from '@angular/core';
import { Produto } from '../../../core/models/Produto';
import { Categoria } from '../../../core/models/categoria/Categoria';
import { ProdutoService } from '../../../core/services/ProdutoService';
import { CategoriaService } from '../../../core/services/CategoriaService';
import { CarrinhoService } from '../../../core/services/CarrinhoService';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-catalogo',
  imports: [CommonModule],
  templateUrl: './catalogo.html',
  styleUrl: './catalogo.scss',
})
export class Catalogo implements OnInit {
  produtos: Produto[] = [];
  categorias: Categoria[] = [];
  categoriaSelecionada: string | null = null;
  carregando = true;

  constructor(
    private produtoService: ProdutoService,
    private categoriaService: CategoriaService,
    public carrinhoService: CarrinhoService
  ) {}
  ngOnInit(): void {
    this.carregarCategorias();
    this.carregarProdutos();
  }

  carregarCategorias(): void {
    this.categoriaService.listarTodas().subscribe({
      next: (dados) => this.categorias = dados
    });
  }

  carregarProdutos(): void {
    this.carregando = true;
    this.produtoService.listarTodos().subscribe({
      next: (dados) => {
        this.produtos = dados;
        this.carregando = false;
      },
      error: () => this.carregando = false
    });
  }

  filtrarPorCategoria(categoriaId: string | null): void {
    this.categoriaSelecionada = categoriaId;

    if (!categoriaId) {
      this.carregarProdutos();
      return;
    }

    this.carregando = true;
    this.produtoService.listarPorCategoria(categoriaId).subscribe({
      next: (dados) => {
        this.produtos = dados;
        this.carregando = false;
      }
    });
  }

  adicionarAoCarrinho(produto: Produto): void {
    this.carrinhoService.adicionarItem(produto);
    console.log('Itens no carrinho:', this.carrinhoService.itensCarrinho());
    
  }
}
