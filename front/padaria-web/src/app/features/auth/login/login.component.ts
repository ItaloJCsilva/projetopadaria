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

  get email() { return this.formulario.get('email')!; }
  get senha() { return this.formulario.get('senha')!; }

  entrar(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    this.carregando = true;
    this.erro = '';

    this.authService.entrar(this.formulario.value).subscribe({
      next: (resposta) => {
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