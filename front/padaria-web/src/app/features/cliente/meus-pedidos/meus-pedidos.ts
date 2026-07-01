import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Pedido } from '../../../core/models/pedido/Pedido';
import { PedidoService } from '../../../core/services/PedidoService';

@Component({
  selector: 'app-meus-pedidos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './meus-pedidos.html'
})
export class MeusPedidos implements OnInit {
  pedidos: Pedido[] = [];
  carregando = false;
  erro = '';

  constructor(private pedidoService: PedidoService) {}

  ngOnInit(): void {
    this.carregar();
  }

  carregar(): void {
    this.carregando = true;
    this.pedidoService.listarMeus().subscribe({
      next: (dados) => { this.pedidos = dados; this.carregando = false; },
      error: () => { this.erro = 'Erro ao carregar pedidos'; this.carregando = false; }
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