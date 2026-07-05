import { Injectable, signal, computed } from '@angular/core';
import { ItemCarrinho } from '../models/pedido/ItemCarrinho';
import { Produto } from '../models/Produto';


@Injectable({ providedIn: 'root' })
export class CarrinhoService {

  private itens = signal<ItemCarrinho[]>([]);

  readonly itensCarrinho = this.itens.asReadonly();
  readonly totalItens = computed(() =>
    this.itens().reduce((acc, i) => acc + i.quantidade, 0));
  readonly totalValor = computed(() =>
    this.itens().reduce((acc, i) => acc + (i.produto.preco * i.quantidade), 0));

  adicionarItem(produto: Produto): void {
    const atual = this.itens();
    const existe = atual.find(i => i.produto.id === produto.id);

    if (existe) {
      this.itens.set(atual.map(i =>
        i.produto.id === produto.id
          ? { ...i, quantidade: i.quantidade + 1 }
          : i
      ));
    } else {
      this.itens.set([...atual, { produto, quantidade: 1 }]);
    }
  }

  removerItem(produtoId: string): void {
    const atual = this.itens();
    const item = atual.find(i => i.produto.id === produtoId);

    if (!item) return;

    if (item.quantidade === 1) {
      this.itens.set(atual.filter(i => i.produto.id !== produtoId));
    } else {
      this.itens.set(atual.map(i =>
        i.produto.id === produtoId
          ? { ...i, quantidade: i.quantidade - 1 }
          : i
      ));
    }
  }

  // Limpa o carrinho completamente
  limpar(): void {
    this.itens.set([]);
  }
}