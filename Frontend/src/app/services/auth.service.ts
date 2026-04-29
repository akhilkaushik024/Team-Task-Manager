import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;
  private currentUserSubject = new BehaviorSubject<any>(JSON.parse(sessionStorage.getItem('user') || 'null'));
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(credentials: any) {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(res => {
        sessionStorage.setItem('token', res.token);
        sessionStorage.setItem('user', JSON.stringify(res.user));
        this.currentUserSubject.next(res.user);
      })
    );
  }

  register(userData: any) {
    return this.http.post<any>(`${this.apiUrl}/register`, userData);
  }

  logout() {
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('user');
    this.currentUserSubject.next(null);
  }

  isAuthenticated(): boolean {
    return !!sessionStorage.getItem('token');
  }

  getToken() {
    return sessionStorage.getItem('token');
  }
}
