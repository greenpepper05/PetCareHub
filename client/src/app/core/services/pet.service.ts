import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { User } from '../../shared/models/user';
import { Pet } from '../../shared/models/pet';

@Injectable({
  providedIn: 'root'
})
export class PetService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);

  createPetProfile(values: any) {
    return this.http.post<Pet>(this.baseUrl + 'pets', values);
  }
}
