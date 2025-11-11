import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Clinic } from '../../shared/models/clinic';
import { Observable } from 'rxjs';
import { ClinicSchedule } from '../../shared/models/clinicSchedule';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getClinicInfo(): Observable<Clinic> {
    return this.http.get<Clinic>(`${this.baseUrl}clinic/admin`);
  }

  getClinicDetail(id: number) {
    return this.http.get<Clinic>(`${this.baseUrl}clinic/info/${id}`);
  }

  getActiveClinic() {
    return this.http.get<Clinic[]>(`${this.baseUrl}clinic/active`);
  }

  getClinicSchedules(id: number){
    return this.http.get<ClinicSchedule[]>(`${this.baseUrl}clinic/${id}/schedule`);
  }

  saveClinicSchedules(id: number, schedules: ClinicSchedule[]) {
    return this.http.post(`${this.baseUrl}clinic/${id}/schedule`, schedules);
  }

  // SUPER ADMIN
  getAllClinic() {
    return this.http.get<Clinic[]>(`${this.baseUrl}clinic`);
  }

  getClinicById(id: number) {
    return this.http.get<Clinic>(`${this.baseUrl}clinic/${id}`);
  }

  updateClinic(id: number, payload: any) {
    return this.http.post<Clinic>(`${this.baseUrl}clinic/update/${id}`, payload);
  }

  registerClinic(payload: any) {
    return this.http.post<Clinic>(`${this.baseUrl}clinic/register`, payload);
  }

  uploadImage(formData: FormData): Observable<{url: string}> {
    return this.http.post<any>(`${this.baseUrl}superadmin/upload`, formData);
  }

  deleteClinic(id: number) {
    return this.http.delete<Clinic>(`${this.baseUrl}clinic/delete/${id}`);
  }
}
