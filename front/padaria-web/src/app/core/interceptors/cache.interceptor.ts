import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { of, tap } from 'rxjs';

const cache = new Map<string, { resposta: HttpResponse<any>, tempo: number }>();
const TEMPO_CACHE_MS = 60000;

export const cacheInterceptor: HttpInterceptorFn = (req, next) => {

  if (req.method !== 'GET') return next(req);

  if (req.url.includes('/autenticacao')) return next(req);

  const agora = Date.now();
  const chave = req.url;
  const cached = cache.get(chave);

  if (cached && (agora - cached.tempo) < TEMPO_CACHE_MS) {
    return of(cached.resposta.clone());
  }

  return next(req).pipe(
    tap(evento => {
      if (evento instanceof HttpResponse) {
        cache.set(chave, { resposta: evento.clone(), tempo: agora });
      }
    })
  );
};