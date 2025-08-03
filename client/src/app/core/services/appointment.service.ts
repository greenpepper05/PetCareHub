import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { User } from '../../shared/models/user';
import { Observable } from 'rxjs';
import { Appointment } from '../../shared/models/appointment';

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

  getAppointmentByClinic() {
    return this.http.get<Appointment[]>(this.baseUrl + 'appointments/clinic');
  }

  
}
