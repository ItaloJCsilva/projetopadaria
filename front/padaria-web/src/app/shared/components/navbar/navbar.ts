import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { CarrinhoService } from '../../../core/services/CarrinhoService';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {

  constructor(
    public authService: AuthService,
    public carrinhoService: CarrinhoService,
    private router: Router
  ) {}

  get logado(): boolean {
    return this.authService.estaLogado();
  }

  get perfil(): string | null {
    return this.authService.obterPerfil();
  }

  get nome(): string | null {
    return this.authService.obterNome();
  }

  sair(): void {
    this.authService.sair();
  }
}