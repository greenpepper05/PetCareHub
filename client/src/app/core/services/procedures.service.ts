import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Procedures } from '../../shared/models/procedures';

@Injectable({
  providedIn: 'root'
})
export class ProceduresService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  getProcedures(procedureId: number) {
    return this.http.get<Procedures[]>(`${this.baseUrl}servicerecord/records/${procedureId}/steps`);
  }
}
