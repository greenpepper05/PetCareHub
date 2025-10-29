import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../../shared/models/user';
import { map } from 'rxjs';
import { AbstractControl, ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  group(arg0: { firstName: (string | ((control: AbstractControl) => ValidationErrors | null))[]; lastName: (string | ((control: AbstractControl) => ValidationErrors | null))[]; email: (string | ((control: AbstractControl) => ValidationErrors | null))[]; contact: (string | ((control: AbstractControl) => ValidationErrors | null))[]; password: (string | ((control: AbstractControl) => ValidationErrors | null))[]; clinicId: null[]; }) {
    throw new Error('Method not implemented.');
  }
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);

  login(values: any) {
    let params = new HttpParams();
    params = params.append('useCookies', true);
    return this.http.post<User>(this.baseUrl + 'login', values, {params});
  }
  
  adminLogin(credentials: {email: string; password: string;}) {
    return this.http.post(`${this.baseUrl}login?useCookies=true`, credentials, {
      withCredentials: true
    })
  }

  register(values: any) {
    return this.http.post(this.baseUrl + 'account/register', values);
  }

  getUserInfo() {
    return this.http.get<User>(this.baseUrl + 'account/user-info', {}).pipe(
      map(user =>{
        this.currentUser.set(user);
        return user;
      })
    )
  }

  logout() {
    return this.http.post(this.baseUrl + 'account/logout', {});
  }

  getAuthState() {
    return this.http.get<{isAuthenticated: boolean}>(this.baseUrl + 'account/auth-status');
  }

  getUserById(id: string) {
    return this.http.get<User>(`${this.baseUrl}account/users/` + id);
  }

  // SUPERADMIN
  getAllAdminUsers() {
    return this.http.get<User[]>(`${this.baseUrl}account/admin-users`);
  }

  getAllUsers() {
    return this.http.get<User[]>(`${this.baseUrl}account/users`);
  }

  registerAdminUser(payload: any) {
    return this.http.post<any>(`${this.baseUrl}account/register/admin`, payload);
  }

  getUserWithClinic(id: string) {
    return this.http.get<User>(`${this.baseUrl}account/user/${id}`);
  }
  
}
