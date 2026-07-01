import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Categoria } from '../models/categoria/Categoria';
import { CriarCategoriaRequisicao } from '../models/categoria/CriarCategoriaRequisicao';

@Injectable({ providedIn: 'root' })
export class CategoriaService {

  private readonly url = 'http://localhost:5002/api/categorias';

  constructor(private http: HttpClient) {}

  listarTodas(): Observable<Categoria[]> {
    return this.http.get<Categoria[]>(this.url);
  }

  buscarPorId(id: string): Observable<Categoria> {
    return this.http.get<Categoria>(`${this.url}/${id}`);
  }

  criar(dados: CriarCategoriaRequisicao): Observable<Categoria> {
    return this.http.post<Categoria>(this.url, dados);
  }

  atualizar(id: string, dados: CriarCategoriaRequisicao): Observable<Categoria> {
    return this.http.put<Categoria>(`${this.url}/${id}`, dados);
  }

  remover(id: string): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}