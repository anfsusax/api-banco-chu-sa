/**
 * EXEMPLO COMPLETO - Angular
 * 
 * Este exemplo mostra uma implementação completa
 * de localização no Angular para a API BankChuSA
 */

// ============================================
// 1. INTERCEPTOR HTTP (adiciona header automaticamente)
// ============================================

import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LanguageService } from './language.service';

@Injectable()
export class LanguageInterceptor implements HttpInterceptor {
  constructor(private languageService: LanguageService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Pegar idioma atual
    const language = this.languageService.getCurrentLanguageValue();
    
    // Clonar requisição e adicionar header
    const clonedRequest = req.clone({
      setHeaders: {
        'Accept-Language': language
      }
    });

    return next.handle(clonedRequest);
  }
}

// ============================================
// 2. SERVIÇO DE IDIOMA
// ============================================

import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private currentLanguage$ = new BehaviorSubject<string>(this.detectLanguage());

  constructor() {
    // Salvar preferência quando mudar
    this.currentLanguage$.subscribe(lang => {
      localStorage.setItem('bankchusa_language', lang);
    });
  }

  private detectLanguage(): string {
    // 1. Verificar localStorage
    const saved = localStorage.getItem('bankchusa_language');
    if (saved) return saved;

    // 2. Usar idioma do navegador
    const browserLang = navigator.language || 'pt-BR';
    
    // 3. Normalizar
    if (browserLang.startsWith('pt')) return 'pt-BR';
    if (browserLang.startsWith('en')) return 'en-US';
    
    return 'pt-BR';
  }

  getCurrentLanguage(): Observable<string> {
    return this.currentLanguage$.asObservable();
  }

  getCurrentLanguageValue(): string {
    return this.currentLanguage$.value;
  }

  setLanguage(language: string) {
    this.currentLanguage$.next(language);
  }
}

// ============================================
// 3. SERVIÇO DA API
// ============================================

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BankChuSAService {
  private baseUrl = 'https://localhost:5001';

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/v1/auth/login`, {
      username,
      password
    });
  }

  getAccount(accountNumber: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/api/v1/accounts/${accountNumber}`);
  }

  transfer(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/v1/transfers`, data);
  }
}

// ============================================
// 4. COMPONENTE DE SELEÇÃO DE IDIOMA
// ============================================

import { Component, OnInit } from '@angular/core';
import { LanguageService } from './language.service';

@Component({
  selector: 'app-language-selector',
  template: `
    <select [value]="currentLanguage" (change)="onLanguageChange($event)">
      <option value="pt-BR">Português</option>
      <option value="en-US">English</option>
    </select>
  `
})
export class LanguageSelectorComponent implements OnInit {
  currentLanguage: string = 'pt-BR';

  constructor(private languageService: LanguageService) {}

  ngOnInit() {
    this.languageService.getCurrentLanguage().subscribe(lang => {
      this.currentLanguage = lang;
    });
  }

  onLanguageChange(event: any) {
    const newLanguage = event.target.value;
    this.languageService.setLanguage(newLanguage);
    // Opcional: Recarregar dados
    window.location.reload();
  }
}

// ============================================
// 5. REGISTRAR NO APP.MODULE.TS
// ============================================

/*
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LanguageInterceptor } from './interceptors/language.interceptor';

@NgModule({
  imports: [HttpClientModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LanguageInterceptor,
      multi: true
    }
  ]
})
export class AppModule { }
*/

// ============================================
// 6. USO NO COMPONENTE
// ============================================

/*
import { Component } from '@angular/core';
import { BankChuSAService } from './services/bankchusa.service';

@Component({
  selector: 'app-login',
  template: `
    <form (ngSubmit)="onLogin()">
      <input [(ngModel)]="username" placeholder="Username">
      <input [(ngModel)]="password" type="password" placeholder="Password">
      <button type="submit">Login</button>
      <div *ngIf="error">{{ error }}</div>
    </form>
  `
})
export class LoginComponent {
  username = '';
  password = '';
  error = '';

  constructor(private api: BankChuSAService) {}

  onLogin() {
    this.api.login(this.username, this.password).subscribe({
      next: (result) => {
        console.log('Token:', result.token);
        // Mensagens de sucesso vêm no idioma correto!
      },
      error: (err) => {
        // Mensagens de erro vêm no idioma correto!
        this.error = err.error?.error || 'Erro ao fazer login';
        // pt-BR: "Usuário ou senha inválidos"
        // en-US: "Invalid username or password"
      }
    });
  }
}
*/

