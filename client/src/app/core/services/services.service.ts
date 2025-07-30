import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ServicesService {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);

  getServices() {
    return this.http.get<any>(this.baseUrl + 'admin/services')
  }
}
