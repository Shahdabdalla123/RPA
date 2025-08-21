import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { API } from './enviroment';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
 private apiUrl = `${API}/Auth`;

constructor(private http:HttpClient,private router:Router) {}

  setAuthState(token: string): void {
    localStorage.setItem('jwtToken', token);
  }

  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, {
      email: email,
      password: password
    });
  }
  logout(): void {
    this.clearAuth();
    this.router.navigate(['/login']);
  }

    isLoggedIn() : boolean {
      return !!localStorage.getItem('jwtToken'); 
    }
    
  getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  clearAuth(): void {
    localStorage.removeItem('jwtToken');
  }


  getUsername(): string | null {
    return localStorage.getItem('username');
  }
}
