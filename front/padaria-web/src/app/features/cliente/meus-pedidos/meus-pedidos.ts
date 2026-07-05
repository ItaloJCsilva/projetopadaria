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

      switch (status) {

          case 'Pendente':
              return 'bg-warning';

          case 'Pronto':
              return 'bg-primary';

          case 'Concluido':
              return 'bg-success';

          default:
              return 'bg-secondary';
      }

  }
  concluirPedido(id: string): void {
    const confirmar = confirm(
      'Você confirma que recebeu este pedido?'
    );

    if (!confirmar) {
      return;
    }

    this.pedidoService.atualizarStatus(
    id,
    {
        novoStatus: 'Concluido'
    }
    )
    .subscribe({
        next: () => {
            alert('Pedido concluído!');
            this.carregar();
        },
        error: erro => {
            alert(
                erro.error?.mensagem ??
                'Erro ao concluir.'
            );
        }
    });

  }
}