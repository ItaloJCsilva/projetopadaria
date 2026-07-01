import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Pedido } from '../../../core/models/pedido/Pedido';
import { PedidoService } from '../../../core/services/PedidoService';


@Component({
  selector: 'app-pedidos-admin',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pedidos.html'
})
export class Pedidos implements OnInit {
  pedidos: Pedido[] = [];
  carregando = false;
  erro = '';

  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.carregar();
  }

  carregar(): void {
    this.carregando = true;
    this.pedidoService.listarTodos().subscribe({
      next: (dados) => { this.pedidos = dados; this.carregando = false; },
      error: () => { this.erro = 'Erro ao carregar pedidos'; this.carregando = false; }
    });
  }

  atualizarStatus(id: string, status: string): void {
    this.pedidoService.atualizarStatus(id, { novoStatus: status }).subscribe({
      next: () => this.carregar(),
      error: () => this.erro = 'Erro ao atualizar status'
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      'Pendente': 'bg-warning',
      'Confirmado': 'bg-info',
      'Pronto': 'bg-primary',
      'Concluido': 'bg-success',
      'Cancelado': 'bg-danger'
    };
    return map[status] || 'bg-secondary';
  }
}