import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styles: [`
    nav { background: var(--surface); border-bottom: 1px solid var(--border); padding: 1rem 2rem; display: flex; justify-content: space-between; align-items: center; }
    .brand { font-size: 1.25rem; font-weight: 700; color: var(--text); background: linear-gradient(to right, #4F46E5, #10B981); -webkit-background-clip: text; -webkit-text-fill-color: transparent; }
    .links { display: flex; gap: 1.5rem; align-items: center; }
    .links a { font-weight: 500; color: var(--text-muted); padding: 0.5rem 0; }
    .links a:hover, .links a.active { color: var(--text); border-bottom: 2px solid var(--primary); }
  `]
})
export class NavbarComponent {
  auth = inject(AuthService);
  router = inject(Router);
  currentUser$ = this.auth.currentUser$;

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
