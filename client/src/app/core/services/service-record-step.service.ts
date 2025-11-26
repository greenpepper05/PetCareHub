import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { ServiceRecordStep } from '../../shared/models/serviceRecordStep';
import { SkipDto } from '../../shared/models/skipDto';

@Injectable({
  providedIn: 'root'
})
export class ServiceRecordStepService {
    private http = inject(HttpClient);
    private baseUrl = environment.apiUrl;

    getServiceRecordStep(recordId: number) {
      return this.http.get<ServiceRecordStep[]>(`${this.baseUrl}servicerecord/records/${recordId}/steps`);
    }

    addServiceRecordStep(recordId: number, steps: ServiceRecordStep) {
      return this.http.post<ServiceRecordStep>(`${this.baseUrl}servicerecord`, steps);
    }

    skipServiceRecordStep(recordId: number, stepId: number, skip: boolean) {
      return this.http.patch<ServiceRecordStep>(`${this.baseUrl}servicerecord/records/${recordId}/steps/${stepId}/skip`, {
        isSkipped: skip
      });
    }
}
