import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { LoginRequisicao } from '../models/LoginRequisicao';
import { LoginResposta } from '../models/LoginResposta';
import { CadastroRequisicao } from '../models/CadastroRequisicao';
import { Usuario } from '../models/Usuario';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private readonly url = 'http://localhost:5001/api/autenticacao';

  readonly usuarioLogado = signal<boolean>(this.estaLogado());

  constructor(private http: HttpClient, private router: Router) {}

  entrar(dados: LoginRequisicao): Observable<LoginResposta> {
    return this.http.post<LoginResposta>(`${this.url}/login`, dados).pipe(
      tap(resposta => {
        localStorage.setItem('token', resposta.token);
        localStorage.setItem('perfil', resposta.perfil);
        localStorage.setItem('nome', resposta.nome ?? resposta.nomeUsuario ?? '');
        this.usuarioLogado.set(true); // ← notifica a navbar
        console.log('[AuthService] login ok, perfil:', resposta.perfil);
      })
    );
  }

  cadastrar(dados: CadastroRequisicao): Observable<Usuario> {
    return this.http.post<Usuario>(`${this.url}/cadastrar`, dados);
  }

  buscarPerfil(): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.url}/Pegar-perfil`);
  }

  sair(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('perfil');
    localStorage.removeItem('nome');
    this.usuarioLogado.set(false); // ← notifica a navbar
    console.log('[AuthService] logout');
    this.router.navigate(['/login']);
  }

  obterToken(): string | null {
    return localStorage.getItem('token');
  }

  estaLogado(): boolean {
    const token = this.obterToken();
    if (!token) return false;
    try {
      const decoded: any = jwtDecode(token);
      return decoded.exp > Date.now() / 1000;
    } catch {
      return false;
    }
  }

  obterPerfil(): string | null {
    return localStorage.getItem('perfil');
  }

  obterNome(): string | null {
    return localStorage.getItem('nome');
  }
}