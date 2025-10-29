import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Clinic } from '../../shared/models/clinic';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getClinicInfo(): Observable<Clinic> {
    return this.http.get<Clinic>(`${this.baseUrl}clinic/admin`);
  }

  // SUPER ADMIN
  getAllClinic() {
    return this.http.get<Clinic[]>(`${this.baseUrl}clinic`);
  }

  getClinicById(id: number) {
    return this.http.get<Clinic>(`${this.baseUrl}clinic/${id}`);
  }

  registerClinic(payload: any) {
    return this.http.post<Clinic>(`${this.baseUrl}clinic/register`, payload);
  }

  uploadImage(formData: FormData): Observable<{url: string}> {
    return this.http.post<any>(`${this.baseUrl}superadmin/upload`, formData);
  }

  deleteClinic(id: number) {
    return this.http.delete<Clinic>(`${this.baseUrl}clinic/${id}`);
  }
}
