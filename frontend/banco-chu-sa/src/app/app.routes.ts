import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'accounts',
    loadComponent: () => import('./pages/accounts/accounts.component').then(m => m.AccountsComponent),
    canActivate: [authGuard]
  },
  {
    path: 'transfers',
    loadComponent: () => import('./pages/transfers/transfers.component').then(m => m.TransfersComponent),
    canActivate: [authGuard]
  },
  {
    path: 'statements',
    loadComponent: () => import('./pages/statements/statements.component').then(m => m.StatementsComponent),
    canActivate: [authGuard]
  },
  {
    path: '',
    redirectTo: '/accounts',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/accounts'
  }
];
