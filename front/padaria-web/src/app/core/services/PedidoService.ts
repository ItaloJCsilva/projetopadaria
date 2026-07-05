import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pedido } from '../models/pedido/Pedido';
import { CriarPedidoRequisicao } from '../models/pedido/CriarPedidoRequisicao';
import { AtualizarStatusRequisicao } from '../models/pedido/AtualizarStatusRequisicao';

@Injectable({ providedIn: 'root' })
export class PedidoService {

  private readonly url = 'http://localhost:5003/api/pedidos';

  constructor(private http: HttpClient) {}

  listarTodos(): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(this.url);
  }

  listarMeus(): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(`${this.url}/meus-pedidos`);
  }

  listarPorStatus(status: string): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(`${this.url}/status/${status}`);
  }

  buscarPorId(id: string): Observable<Pedido> {
    return this.http.get<Pedido>(`${this.url}/${id}`);
  }

  criar(dados: CriarPedidoRequisicao): Observable<Pedido> {
    return this.http.post<Pedido>(this.url, dados);
  }

  atualizarStatus(
      id: string,
      dados: AtualizarStatusRequisicao
  ): Observable<Pedido> {

      return this.http.put<Pedido>(
          `${this.url}/${id}/status`,
          dados
      );
  }
  // cancelar(id: string): Observable<void> {
  //   return this.http.delete<void>(`${this.url}/${id}`);
  // }
  // concluir(id: string): Observable<void> {
  // return this.http.put<void>(`${this.url}/${id}/concluir`, {});
  // }
  listarAtivos(): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(`${this.url}/ativos`);
  }
  // confirmar(id: string): Observable<void> {
  // return this.http.put<void>(`${this.url}/${id}/confirmar`, {});

}