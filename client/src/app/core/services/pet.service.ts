import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../../shared/models/user';
import { Pet } from '../../shared/models/pet';
import { Pagination } from '../../shared/models/pagination';
import { PetParams } from '../../shared/models/petParams';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PetService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);

  createPetProfile(payload: any) {
    return this.http.post<Pet>(this.baseUrl + 'pets', payload);
  }

  getPetsByOwner() {
    return this.http.get<Pet[]>(this.baseUrl + 'pets');
  }

  getPetByClinic(clinicId: number) {
    return this.http.get<Pet[]>(`${this.baseUrl}pets/${clinicId}/all-pets`);
  }

  getAllPetByClinic(petParams: PetParams, clinicId: number) {
    let params = new HttpParams();

    params = params.append('pageSize', petParams.pageSize);
    params = params.append('pageIndex', petParams.pageNumber);

    if (petParams.search) {
      params = params.append('search', petParams.search);
    }
    return this.http.get<Pagination<Pet>>(`${this.baseUrl}pets/${clinicId}/paginated`, {params});
  }

  getPetById(id: number) {
    
    return this.http.get<Pet>(`${this.baseUrl}pets/${id}`);
  }

  uploadImage(formData: FormData): Observable<{ url: string}> {
    return this.http.post<any>(`${this.baseUrl}pets/upload/pet`, formData);
  }

  // SuperAdmin
  getAllPets() {
    return this.http.get<Pet[]>(`${this.baseUrl}pets/all`);
  }


}
