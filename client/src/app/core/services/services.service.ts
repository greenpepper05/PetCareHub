import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Services } from '../../shared/models/services';

@Injectable({
  providedIn: 'root'
})
export class ServicesService {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);

  getServices(): Observable<Services[]> {
    return this.http.get<Services[]>(this.baseUrl + 'services')
  }

  getService(id: number) {
    return this.http.get<any>(this.baseUrl + 'services/' + id);
  }

  getServiceByClinicId() {
    return this.http.get<Services[]>(`${this.baseUrl}services/clinic`);
  }

  createService(payload: any) {
    return this.http.post<Services>(`${this.baseUrl}services`, payload);
  }
}
