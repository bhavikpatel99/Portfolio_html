import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () =>
      import('./components/login/login.component').then((m) => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./components/register/register.component').then((m) => m.RegisterComponent)
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/dashboard/dashboard.component').then((m) => m.DashboardComponent)
  },
  {
    path: 'candidate/new',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/candidate-form/candidate-form.component').then((m) => m.CandidateFormComponent)
  },
  {
    path: 'candidate/:id',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/candidate-form/candidate-form.component').then((m) => m.CandidateFormComponent)
  },
  { path: '**', redirectTo: 'login' }
];
