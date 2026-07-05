import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';


export const atendenteGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const perfil = auth.obterPerfil();
  if (perfil === 'Atendente' || perfil === 'Administrador') return true;

  router.navigate(['/']);
  return false;
};