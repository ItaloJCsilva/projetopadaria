import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html'
})
export class LoginComponent {

  // FormGroup — agrupa os campos do formulário
  // Validators.required → campo obrigatório
  // Validators.email → valida formato de email
  formulario: FormGroup;
  carregando = false;
  erro = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.formulario = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  // Getter para acessar os campos no template
  get email() { return this.formulario.get('email')!; }
  get senha() { return this.formulario.get('senha')!; }

  entrar(): void {
    // Se formulário inválido não submete
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    this.carregando = true;
    this.erro = '';

    this.authService.entrar(this.formulario.value).subscribe({
      next: (resposta) => {
        // Redireciona por perfil após login
        const perfil = resposta.perfil;
        if (perfil === 'Administrador') this.router.navigate(['/admin']);
        else if (perfil === 'Atendente') this.router.navigate(['/caixa']);
        else this.router.navigate(['/catalogo']);
      },
      error: () => {
        this.erro = 'Email ou senha inválidos.';
        this.carregando = false;
      }
    });
  }
}