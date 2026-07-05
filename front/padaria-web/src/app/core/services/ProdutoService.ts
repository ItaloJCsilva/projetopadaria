import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Produto } from '../models/Produto';
import { CriarProdutoRequisicao } from '../models/CriarProdutoRequisicao';
import { AtualizarProdutoRequisicao } from '../models/AtualizarProdutoRequisicao';


@Injectable({ providedIn: 'root' })
export class ProdutoService {

  private readonly url = 'http://localhost:5002/api/produtos';

  constructor(private http: HttpClient) {}

  listarTodos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.url);
  }

  listarPorCategoria(categoriaId: string): Observable<Produto[]> {
    return this.http.get<Produto[]>(`${this.url}/categoria/${categoriaId}`);
  }

  buscarPorId(id: string): Observable<Produto> {
    return this.http.get<Produto>(`${this.url}/${id}`);
  }

  criar(dados: CriarProdutoRequisicao): Observable<Produto> {
    return this.http.post<Produto>(this.url, dados);
  }

  atualizar(id: string, dados: AtualizarProdutoRequisicao): Observable<Produto> {
    return this.http.put<Produto>(`${this.url}/${id}`, dados);
  }

  remover(id: string): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
  criarComImagem(formData: FormData) {
  return this.http.post(this.url, formData);
}

atualizarComImagem(id: string, formData: FormData) {
  return this.http.put(`${this.url}/${id}`, formData);
}
}