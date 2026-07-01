// Interceptor de autenticação
// Adiciona o token JWT em TODAS as requisições HTTP automaticamente
// Sem isso precisaria adicionar o header manualmente em cada chamada
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';


export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.obterToken();

  // Se não tem token manda a requisição sem autenticação
  if (!token) return next(req);

  // Clona a requisição adicionando o header Authorization
  const reqAutenticada = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`)
  });

  return next(reqAutenticada);
};