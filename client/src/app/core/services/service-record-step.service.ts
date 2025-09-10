import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { ServiceRecordStep } from '../../shared/models/serviceRecordStep';

@Injectable({
  providedIn: 'root'
})
export class ServiceRecordStepService {
    private http = inject(HttpClient);
    private baseUrl = environment.apiUrl;

    getServiceRecordStep(recordId: number) {
      return this.http.get<ServiceRecordStep[]>(`${this.baseUrl}servicerecord/records/${recordId}/steps`);
    }

}
