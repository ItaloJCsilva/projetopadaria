export interface Produto {
  id: string;
  nome: string;
  descricao: string;
  preco: number;
  estoque: number;
  urlImagem: string;
  disponivel: boolean;
  categoriaId: string;
  nomeCategoria: string;
  criadoEm: string;
}