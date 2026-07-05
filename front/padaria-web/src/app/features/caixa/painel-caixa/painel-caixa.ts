import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Produto } from '../../../core/models/Produto';
import { ProdutoService } from '../../../core/services/ProdutoService';
import { PedidoService } from '../../../core/services/PedidoService';
import { AuthService } from '../../../core/services/auth.service';
import { CriarPedidoRequisicao } from '../../../core/models/pedido/CriarPedidoRequisicao';

@Component({
  selector: 'app-painel-caixa',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './painel-caixa.html'
})
export class PainelCaixa implements OnInit {

  produtos: Produto[] = [];
  carrinho: { produto: Produto; quantidade: number }[] = [];
  
  pedidos: any[] = [];

  formularioCliente: FormGroup;

  enviando = false;
  erro = '';
  sucesso = '';

  constructor(
    private produtoService: ProdutoService,
    private pedidoService: PedidoService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.formularioCliente = this.fb.group({
      nomeCliente: ['', Validators.required],
      emailCliente: ['', [Validators.required, Validators.email]],
      telefoneCliente: ['', Validators.required],
      observacoes: ['']
    });
  }

  ngOnInit(): void {
    this.produtoService.listarTodos().subscribe({
      next: (dados) => this.produtos = dados,
      error: () => this.erro = 'Erro ao carregar produtos'
    });

    this.carregarPedidos();
  }

  adicionar(produto: Produto): void {
    const existente = this.carrinho.find(i => i.produto.id === produto.id);

    if (existente) {
      existente.quantidade++;
    } else {
      this.carrinho.push({ produto, quantidade: 1 });
    }
  }

  remover(produtoId: string): void {
    const item = this.carrinho.find(i => i.produto.id === produtoId);

    if (!item) return;

    if (item.quantidade === 1) {
      this.carrinho = this.carrinho.filter(i => i.produto.id !== produtoId);
    } else {
      item.quantidade--;
    }
  }

  get total(): number {
    return this.carrinho.reduce(
      (acc, i) => acc + i.produto.preco * i.quantidade,
      0
    );
  }

  finalizarPedido(): void {

    if (this.formularioCliente.invalid) {
      this.formularioCliente.markAllAsTouched();
      return;
    }

    if (this.carrinho.length === 0) {
      this.erro = 'Carrinho vazio.';
      return;
    }

    this.enviando = true;

    const dados: CriarPedidoRequisicao = {
      usuarioId: null,
      nomeCliente: this.formularioCliente.value.nomeCliente,
      emailCliente: this.formularioCliente.value.emailCliente,
      telefoneCliente: this.formularioCliente.value.telefoneCliente,
      tipo: 'Local',
      observacoes: this.formularioCliente.value.observacoes || null,
      itens: this.carrinho.map(item => ({
        produtoId: item.produto.id,
        nomeProduto: item.produto.nome,
        quantidade: item.quantidade,
        precoUnitario: item.produto.preco
      }))
    };

    this.pedidoService.criar(dados).subscribe({
      next: () => {
        this.sucesso = 'Pedido criado com sucesso!';
        this.carrinho = [];
        this.formularioCliente.reset();
        this.enviando = false;

        this.carregarPedidos();
      },
      error: () => {
        this.erro = 'Erro ao finalizar pedido.';
        this.enviando = false;
      }
    });
  }

  carregarPedidos(): void {
    this.pedidoService.listarAtivos().subscribe({
      next: (dados) => this.pedidos = dados.filter(p => p.status !== 'Concluido'),
      error: () => this.erro = 'Erro ao carregar pedidos'
    });
  }

//   confirmar(id: string): void {
//     this.pedidoService.atualizarStatus(id, {
//       novoStatus: 'Confirmado'
//     }).subscribe(() => this.carregarPedidos());
//   }

//   pronto(id: string): void {
//     this.pedidoService.atualizarStatus(id, {
//       novoStatus: 'Pronto'
//     }).subscribe(() => this.carregarPedidos());
//   }

//   concluir(id: string): void {
//     this.pedidoService.concluir(id).subscribe(() => this.carregarPedidos());
//   }
// }
  proximoStatus(pedido: any): void {

    let novoStatus = '';

    switch (pedido.status) {

      case 'Pendente':
        novoStatus = 'Pronto';
        break;

      case 'Pronto':
        novoStatus = 'Concluido';
        break;

      default:
        return;
    }

    this.pedidoService.atualizarStatus(
      pedido.id,
      {
        novoStatus
      }
    ).subscribe(() => {

      this.carregarPedidos();

    });

  }
}