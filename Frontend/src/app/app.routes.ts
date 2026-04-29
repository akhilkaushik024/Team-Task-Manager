import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProjectsComponent } from './components/projects/projects.component';
import { TasksComponent } from './components/tasks/tasks.component';
import { UsersComponent } from './components/users/users.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'projects', component: ProjectsComponent, canActivate: [authGuard] },
  { path: 'tasks', component: TasksComponent, canActivate: [authGuard] },
  { path: 'users', component: UsersComponent, canActivate: [authGuard] },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' }
];
