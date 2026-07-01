import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Categoria } from '../../../core/models/categoria/Categoria';
import { CategoriaService } from '../../../core/services/CategoriaService';


@Component({
  selector: 'app-categorias',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './categorias.html'
})
export class Categorias implements OnInit {
  categorias: Categoria[] = [];
  formulario: FormGroup;
  editandoId: string | null = null;
  carregando = false;
  erro = '';
  sucesso = '';

  constructor(private categoriaService: CategoriaService, private fb: FormBuilder) {
    this.formulario = this.fb.group({
      nome: ['', Validators.required],
      descricao: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.carregar();
  }

  carregar(): void {
    this.carregando = true;
    this.categoriaService.listarTodas().subscribe({
      next: (dados) => { this.categorias = dados; this.carregando = false; },
      error: () => { this.erro = 'Erro ao carregar categorias'; this.carregando = false; }
    });
  }

  editar(cat: Categoria): void {
    this.editandoId = cat.id;
    this.formulario.patchValue(cat);
  }

  cancelar(): void {
    this.editandoId = null;
    this.formulario.reset();
  }

  salvar(): void {
    if (this.formulario.invalid) return;
    this.carregando = true;
    const dados = this.formulario.value;

    const op = this.editandoId
      ? this.categoriaService.atualizar(this.editandoId, dados)
      : this.categoriaService.criar(dados);

    op.subscribe({
      next: () => {
        this.sucesso = this.editandoId ? 'Categoria atualizada!' : 'Categoria criada!';
        this.carregar();
        this.cancelar();
        this.carregando = false;
      },
      error: () => { this.erro = 'Erro ao salvar categoria'; this.carregando = false; }
    });
  }

  excluir(id: string): void {
    if (!confirm('Excluir categoria?')) return;
    this.categoriaService.remover(id).subscribe({
      next: () => { this.sucesso = 'Categoria excluída!'; this.carregar(); },
      error: () => this.erro = 'Erro ao excluir categoria.'
    });
  }
}