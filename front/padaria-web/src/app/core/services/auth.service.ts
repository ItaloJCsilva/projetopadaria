
import { Injectable } from '@angular/core';
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

  // URL base do AuthService no Docker ou local
  private readonly url = 'http://localhost:5001/api/autenticacao';

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  // Faz login e salva o token no localStorage
  entrar(dados: LoginRequisicao): Observable<LoginResposta> {
    return this.http.post<LoginResposta>(`${this.url}/login`, dados).pipe(
      tap(resposta => {
        localStorage.setItem('token', resposta.token);
        localStorage.setItem('perfil', resposta.perfil);
        localStorage.setItem('nome', resposta.nome);
      })
    );
  }

  // Cadastra novo usuário
  cadastrar(dados: CadastroRequisicao): Observable<Usuario> {
    return this.http.post<Usuario>(`${this.url}/cadastrar`, dados);
  }

  // Busca perfil do usuário logado
  buscarPerfil(): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.url}/perfil`);
  }

  // Remove token e redireciona para login
  sair(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('perfil');
    localStorage.removeItem('nome');
    this.router.navigate(['/login']);
  }

  // Retorna o token salvo
  obterToken(): string | null {
    return localStorage.getItem('token');
  }

  // Verifica se o usuário está logado
  estaLogado(): boolean {
    const token = this.obterToken();
    if (!token) return false;

    try {
      const decoded: any = jwtDecode(token);
      // Verifica se o token não expirou
      return decoded.exp > Date.now() / 1000;
    } catch {
      return false;
    }
  }

  // Retorna o perfil do usuário logado
  obterPerfil(): string | null {
    return localStorage.getItem('perfil');
  }

  // Retorna o nome do usuário logado
  obterNome(): string | null {
    return localStorage.getItem('nome');
  }
}