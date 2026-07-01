import { Produto } from "../Produto";

export interface ItemCarrinho {
  produto: Produto;
  quantidade: number;
}