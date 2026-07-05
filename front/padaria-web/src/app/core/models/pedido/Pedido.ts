import { ItemPedido } from "./ItemPedido";

export interface Pedido {
  id: string;
  usuarioId: string | null;
  nomeCliente: string;
  emailCliente: string;
  telefoneCliente: string;
  tipo: 'Online' | 'Local';
  status: 'Pendente' | 'Pronto' | 'Concluido';
  total: number;
  observacoes: string | null;
  criadoEm: string;
  atualizadoEm: string | null;
  itens: ItemPedido[];
}