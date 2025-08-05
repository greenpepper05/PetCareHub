import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { PetServiceHistory } from '../../shared/models/petServiceHistory';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PetServiceHistoryService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  createPetServiceHistory(history: Partial<PetServiceHistory>) {
    return this.http.post<PetServiceHistory>(this.baseUrl + 'petservicehistory', history);
  }

  getPetHistoryByPetId(petId: number): Observable<PetServiceHistory[]> {
    return this.http.get<PetServiceHistory[]>(`${this.baseUrl}petservicehistory/${petId}`)
  }

  getAll(): Observable<PetServiceHistory[]> {
    return this.http.get<PetServiceHistory[]>(`${this.baseUrl}petservicehistory`);
  }

}
