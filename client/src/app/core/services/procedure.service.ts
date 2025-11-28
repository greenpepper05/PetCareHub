import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ServiceRecordStep } from '../../shared/models/serviceRecordStep';
import { ServiceRecord } from '../../shared/models/serviceRecord';
import { Procedures } from '../../shared/models/procedures';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProcedureService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  stepComplete(recordId: number, stepId: number) {
    return this.http.put<ServiceRecordStep>(`${this.baseUrl}servicerecord/records/${recordId}/steps/${stepId}/complete`, {});
  }

  procedureComplete(serviceId: number) {
    return this.http.patch<ServiceRecord>(`${this.baseUrl}servicerecord/records/${serviceId}`, {});
  }

  getProcedures(serviceId: number) {
      return this.http.get<Procedures[]>(`${this.baseUrl}clinic/services/${serviceId}/procedures`);
  }

  reorderProcedures(serviceId: number, procedures: Procedures[]): Observable<any> {
    const procedureDtos = procedures.map(p => ({
      id: p.id,
      order: p.order
    }));
    return this.http.patch(`${this.baseUrl}clinic/services/${serviceId}/procedures/reorder`, procedureDtos);
  }

  createProcedure(payload: any, serviceId: number) {
    return this.http.post<Procedures>(`${this.baseUrl}clinic/services/${serviceId}/procedures`, payload);
  }

  deleteProcedure(id: number) {
    return this.http.delete<Procedures>(`${this.baseUrl}clinic/procedures/${id}`);
  }

  updateProcedure(procedure: Procedures) : Observable<Procedures> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json'})
    return this.http.patch<Procedures>(`${this.baseUrl}clinic/procedures/${procedure.id}`, procedure, {headers});
  }
}
