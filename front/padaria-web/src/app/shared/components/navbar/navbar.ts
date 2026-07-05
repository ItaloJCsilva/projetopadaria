import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { CarrinhoService } from '../../../core/services/CarrinhoService';

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
    public carrinhoService: CarrinhoService
  ) {
    console.log('[Navbar] iniciou, logado:', this.authService.usuarioLogado());
  }

  sair(): void {
    this.authService.sair();
  }
}