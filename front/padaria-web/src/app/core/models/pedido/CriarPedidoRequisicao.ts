import { ItemPedidoRequisicao } from "./ItemPedidoRequisicao";

export interface CriarPedidoRequisicao {
  usuarioId: string | null;
  nomeCliente: string;
  emailCliente: string;
  telefoneCliente: string;
  tipo: 'Online' | 'Local';
  observacoes: string | null;
  itens: ItemPedidoRequisicao[];
}