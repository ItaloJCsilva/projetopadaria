// Guard de administrador
// Bloqueia acesso ao módulo admin para não-administradores
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';


export const adminGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.obterPerfil() === 'Administrador') return true;

  router.navigate(['/']);
  return false;
};