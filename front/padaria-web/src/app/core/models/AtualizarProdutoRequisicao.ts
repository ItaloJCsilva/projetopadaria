export interface AtualizarProdutoRequisicao {
  nome: string;
  descricao: string;
  preco: number;
  estoque: number;
  urlImagem: string;
  disponivel: boolean;
  categoriaId: string;
}