import { HttpInterceptorFn } from '@angular/common/http';

export const languageInterceptor: HttpInterceptorFn = (req, next) => {
  const language = navigator.language.startsWith('pt') ? 'pt-BR' : 'en-US';
  
  const cloned = req.clone({
    setHeaders: {
      'Accept-Language': language
    }
  });
  
  return next(cloned);
};

