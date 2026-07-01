import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';
import { atendenteGuard } from './core/guards/atendente.guard';

export const routes: Routes = [
  // Rota padrão abre a homepage
  { path: '', redirectTo: '/home', pathMatch: 'full' },

  // Homepage
  {
    path: 'home',
    loadComponent: () => import('./features/publico/homepage/homepage/homepage')
      .then(m => m.Homepage)
  },

  // Autenticação
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component')
      .then(m => m.LoginComponent)
  },
  {
    path: 'cadastro',
    loadComponent: () => import('./features/auth/cadastro/cadastro.component')
      .then(m => m.CadastroComponent)
  },

  // Catálogo público
  {
    path: 'catalogo',
    loadComponent: () => import('./features/publico/catalogo/catalogo')
      .then(m => m.Catalogo)
  },

  // Carrinho
  {
    path: 'carrinho',
    loadComponent: () => import('./features/publico/carrinho/carrinho')
      .then(m => m.Carrinho)
  },

  // Meus pedidos — cliente logado
  {
    path: 'meus-pedidos',
    canActivate: [authGuard],
    loadComponent: () => import('./features/cliente/meus-pedidos/meus-pedidos')
      .then(m => m.MeusPedidos)
  },

  // Caixa — Atendente ou Administrador
  {
    path: 'caixa',
    canActivate: [authGuard, atendenteGuard],
    loadComponent: () => import('./features/caixa/painel-caixa/painel-caixa')
      .then(m => m.PainelCaixa)
  },

  // Admin — só Administrador
  {
    path: 'admin',
    canActivate: [authGuard, adminGuard],
    children: [
      { path: '', redirectTo: 'produtos', pathMatch: 'full' },
      {
        path: 'estoque',
        loadComponent: () => import('./features/admin/estoque/estoque')
          .then(m => m.Estoque)
      },
      {
        path: 'produtos',
        loadComponent: () => import('./features/admin/produtos/produtos')
          .then(m => m.Produtos)
      },
      {
        path: 'categorias',
        loadComponent: () => import('./features/admin/categorias/categorias')
          .then(m => m.Categorias)
      },
      {
        path: 'pedidos',
        loadComponent: () => import('./features/admin/pedidos/pedidos')
          .then(m => m.Pedidos)
      }
    ]
  },

  // Rota não encontrada — volta para home
  { path: '**', redirectTo: '/home' }
];