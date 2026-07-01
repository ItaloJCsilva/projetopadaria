import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';


@Component({
  selector: 'app-cadastro',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './cadastro.component.html'
})
export class CadastroComponent {

  formulario: FormGroup;
  carregando = false;
  erro = '';
  sucesso = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.formulario = this.fb.group({
      nomeUsuario: ['', [Validators.required, Validators.minLength(3)]],
      nome:        ['', Validators.required],
      email:       ['', [Validators.required, Validators.email]],
      senha:       ['', [Validators.required, Validators.minLength(6)]],
      telefone:    ['', Validators.required]
    });
  }

  get nomeUsuario() { return this.formulario.get('nomeUsuario')!; }
  get nome()        { return this.formulario.get('nome')!; }
  get email()       { return this.formulario.get('email')!; }
  get senha()       { return this.formulario.get('senha')!; }
  get telefone()    { return this.formulario.get('telefone')!; }

  cadastrar(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    this.carregando = true;
    this.erro = '';

    this.authService.cadastrar(this.formulario.value).subscribe({
      next: () => {
        this.sucesso = 'Cadastro realizado! Redirecionando...';
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (err) => {
        this.erro = err.error?.mensagem || 'Erro ao cadastrar. Tente novamente.';
        this.carregando = false;
      }
    });
  }
}