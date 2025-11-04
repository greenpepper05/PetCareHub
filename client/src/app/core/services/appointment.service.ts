import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../../shared/models/user';
import { Observable } from 'rxjs';
import { Appointment } from '../../shared/models/appointment';
import { Pagination } from '../../shared/models/pagination';
import { AppointmentParams } from '../../shared/models/appointmentParams';
import { UpcomingAppointmentsParams } from '../../shared/models/upcomingAppointmentParams';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);
  appointmentComplete = false;
  appointmentSignal = signal<Appointment | null>(null);

  createAppointment(payload: any): Observable<Appointment> {
    return this.http.post<Appointment>(this.baseUrl + 'appointments', payload);
  }

  getAppointment(id: number): Observable<Appointment> {
    return this.http.get<Appointment>(`${this.baseUrl}appointments/${id}`);
  }

  getAppointmentForUser(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(this.baseUrl + 'appointments');
  }

  getAppointmentDetail(id: number) {
    return this.http.get<Appointment>(this.baseUrl + 'appointments/' + id);
  }

  getAllAppointments(): Observable<Appointment[]>{
    return this.http.get<Appointment[]>(this.baseUrl + 'appointments');
  }

  updateStatus(id: number, status: string) {
    return this.http.patch<Appointment>(`${this.baseUrl}appointments/${id}/status/`, {status});
  }

  getAllAppointmentByClinicId(id: number) {
    return this.http.get<Appointment[]>(`${this.baseUrl}appointments/${id}/all`);
  }

  getUpcomingAppointment(upcomingParams: UpcomingAppointmentsParams, clinicId: number) {
    let params = new HttpParams();

    params = params.append('pageSize', upcomingParams.pageSize);  
    params = params.append('pageIndex', upcomingParams.pageNumber);
    
    return this.http.get<Pagination<Appointment>>(`${this.baseUrl}appointments/upcoming`, { params });
  }

  getAppointmentByClinic(appointmentParams: AppointmentParams, date: Date) {
    let params = new HttpParams();

    if (appointmentParams.sort) {
      params = params.append('sort', appointmentParams.sort);
    }

    if (appointmentParams.search) {
      params = params.append('search', appointmentParams.search);
    }

    params = params.append('pageSize', appointmentParams.pageSize);  
    params = params.append('pageIndex', appointmentParams.pageNumber);
    const formattedDate = date.toLocaleDateString('en-CA').split('T')[0];
    return this.http.get<Pagination<Appointment>>(this.baseUrl + 'appointments/clinic/by-date?date=' + formattedDate, { params });
  }

  // SuperAdmin
  getAllAppointment() {
    return this.http.get<Appointment[]>(`${this.baseUrl}appointments/all`);
  }
  getAllConfirmedAppointment() {
    return this.http.get<Appointment[]>(`${this.baseUrl}appointments/all-confirmed`);
  }
}
