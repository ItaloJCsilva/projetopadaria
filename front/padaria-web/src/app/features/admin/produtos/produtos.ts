import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Produto } from '../../../core/models/Produto';
import { Categoria } from '../../../core/models/categoria/Categoria';
import { ProdutoService } from '../../../core/services/ProdutoService';
import { CategoriaService } from '../../../core/services/CategoriaService';
import { CriarProdutoRequisicao } from '../../../core/models/CriarProdutoRequisicao';

@Component({
  selector: 'app-produtos',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './produtos.html',
  styleUrl: './produtos.scss',
})
export class Produtos {
  produtos: Produto[] = [];
  categorias: Categoria[] = [];
  formulario: FormGroup;
  editandoId: string | null = null;
  carregando = false;
  erro = '';
  sucesso = '';

  constructor(
    private produtoService: ProdutoService,
    private categoriaService: CategoriaService,
    private fb: FormBuilder
  ) {
    this.formulario = this.fb.group({
      nome: ['', Validators.required],
      descricao: ['', Validators.required],
      preco: [0, [Validators.required, Validators.min(0.01)]],
      estoque: [0, [Validators.required, Validators.min(0)]],
      urlImagem: [''],
      categoriaId: ['', Validators.required],
      disponivel: [true]
    });
  }

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados(): void {
    this.carregando = true;
    this.produtoService.listarTodos().subscribe({
      next: (dados) => { this.produtos = dados; this.carregando = false; },
      error: () => { this.erro = 'Erro ao carregar produtos'; this.carregando = false; }
    });
    this.categoriaService.listarTodas().subscribe({
      next: (dados) => this.categorias = dados
    });
  }

  editar(produto: Produto): void {
    this.editandoId = produto.id;
    this.formulario.patchValue({
      nome: produto.nome,
      descricao: produto.descricao,
      preco: produto.preco,
      estoque: produto.estoque,
      urlImagem: produto.urlImagem,
      categoriaId: produto.categoriaId,
      disponivel: produto.disponivel
    });
  }

  cancelarEdicao(): void {
    this.editandoId = null;
    this.formulario.reset({ disponivel: true });
    this.erro = '';
    this.sucesso = '';
  }

  salvar(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    const dados = this.formulario.value as CriarProdutoRequisicao;
    this.carregando = true;
    this.erro = '';

    const operacao = this.editandoId
      ? this.produtoService.atualizar(this.editandoId, { ...dados, disponivel: this.formulario.value.disponivel } as any)
      : this.produtoService.criar(dados);

    operacao.subscribe({
      next: () => {
        this.sucesso = this.editandoId ? 'Produto atualizado!' : 'Produto criado!';
        this.carregarDados();
        this.cancelarEdicao();
        this.carregando = false;
      },
      error: (err) => {
        this.erro = err.error?.mensagem || 'Erro ao salvar produto.';
        this.carregando = false;
      }
    });
  }

  excluir(id: string): void {
    if (!confirm('Tem certeza que deseja excluir este produto?')) return;
    this.produtoService.remover(id).subscribe({
      next: () => {
        this.sucesso = 'Produto excluído!';
        this.carregarDados();
      },
      error: () => this.erro = 'Erro ao excluir produto.'
    });
  }

}
