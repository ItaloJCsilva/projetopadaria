import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CarrinhoService } from '../../../core/services/CarrinhoService';
import { PedidoService } from '../../../core/services/PedidoService';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { CriarPedidoRequisicao } from '../../../core/models/pedido/CriarPedidoRequisicao';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-carrinho',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './carrinho.html',
  styleUrl: './carrinho.scss',
})
export class Carrinho {
  formulario: FormGroup;
  enviando = false;
  erro = '';
  sucesso = '';

  constructor(
    public carrinhoService: CarrinhoService,
    private pedidoService: PedidoService,
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router
    
  ) {
    this.formulario = this.fb.group({
      nomeCliente: ['', Validators.required],
      emailCliente: ['', [Validators.required, Validators.email]],
      telefoneCliente: ['', Validators.required],
      observacoes: ['']
    });
    console.log('Itens ao abrir carrinho:', this.carrinhoService.itensCarrinho());
  }

  get nomeCliente() { return this.formulario.get('nomeCliente')!; }
  get emailCliente() { return this.formulario.get('emailCliente')!; }
  get telefoneCliente() { return this.formulario.get('telefoneCliente')!; }

  aumentar(produtoId: string, produto: any): void {
    this.carrinhoService.adicionarItem(produto);
  }

  diminuir(produtoId: string): void {
    this.carrinhoService.removerItem(produtoId);
  }

  finalizarPedido(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    if (this.carrinhoService.itensCarrinho().length === 0) {
      this.erro = 'Seu carrinho está vazio.';
      return;
    }

    this.enviando = true;
    this.erro = '';

    const dados: CriarPedidoRequisicao = {
      usuarioId: null,
      nomeCliente: this.formulario.value.nomeCliente,
      emailCliente: this.formulario.value.emailCliente,
      telefoneCliente: this.formulario.value.telefoneCliente,
      tipo: 'Online',
      observacoes: this.formulario.value.observacoes || null,
      itens: this.carrinhoService.itensCarrinho().map(item => ({
        produtoId: item.produto.id,
        nomeProduto: item.produto.nome,
        quantidade: item.quantidade,
        precoUnitario: item.produto.preco
      }))
    };

    this.pedidoService.criar(dados).subscribe({
      next: () => {
        this.sucesso = 'Pedido realizado com sucesso! Você será notificado por email.';
        this.carrinhoService.limpar();
        this.enviando = false;
        setTimeout(() => this.router.navigate(['/catalogo']), 3000);
      },
      error: () => {
        this.erro = 'Erro ao finalizar o pedido. Tente novamente.';
        this.enviando = false;
      }
    });
  }
}
