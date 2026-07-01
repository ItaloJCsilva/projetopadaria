// Interceptor de cache — requisito (viii) do trabalho
// Armazena respostas GET em memória por 60 segundos
// Evita requisições repetidas ao backend para dados que não mudam com frequência
import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { of, tap } from 'rxjs';

// Cache em memória — chave: URL, valor: { resposta, timestamp }
const cache = new Map<string, { resposta: HttpResponse<any>, tempo: number }>();
const TEMPO_CACHE_MS = 60000; // 60 segundos

export const cacheInterceptor: HttpInterceptorFn = (req, next) => {

  // Só aplica cache em requisições GET
  // POST, PUT e DELETE nunca usam cache
  if (req.method !== 'GET') return next(req);

  // Não aplica cache em endpoints de autenticação
  if (req.url.includes('/autenticacao')) return next(req);

  const agora = Date.now();
  const chave = req.url;
  const cached = cache.get(chave);

  // Se tem cache válido retorna sem fazer requisição ao backend
  if (cached && (agora - cached.tempo) < TEMPO_CACHE_MS) {
    return of(cached.resposta.clone());
  }

  // Faz a requisição e salva no cache
  return next(req).pipe(
    tap(evento => {
      if (evento instanceof HttpResponse) {
        cache.set(chave, { resposta: evento.clone(), tempo: agora });
      }
    })
  );
};