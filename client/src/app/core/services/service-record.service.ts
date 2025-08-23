import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { ServiceRecord } from '../../shared/models/serviceRecord';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServiceRecordService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  getServiceRecordByDateAndClinicId(date: Date) {
    const formattedDate = date.toLocaleDateString('en-CA').split('T')[0];
    return this.http.get<ServiceRecord[]>(`${this.baseUrl}servicerecord/by-date?date=${formattedDate}`);
  }

  createServiceRecord(payload: any): Observable<ServiceRecord> {
    return this.http.post<ServiceRecord>(this.baseUrl + 'servicerecord', payload);
  }

  getAll(): Observable<ServiceRecord[]> {
    return this.http.get<ServiceRecord[]>(`${this.baseUrl}servicerecord`);
  }

  getServiceRecordByPetId(id: number){
      return this.http.get<ServiceRecord>(this.baseUrl + 'servicerecord/' + id);
  }
}
