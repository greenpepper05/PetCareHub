import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Staff } from '../../shared/models/staff';
import { CreateStaff } from '../../shared/models/createStaff';

@Injectable({
  providedIn: 'root'
})
export class StaffService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getAllStaff(clinicId: number): Observable<Staff[]> {
    let params = new HttpParams().set('clinicId', clinicId);
    return this.http.get<Staff[]>(`${this.baseUrl}staff`, { params });
  }

  getStaffById(id: number, clinicId: number): Observable<Staff> {
    let params = new HttpParams().set('clinicId', clinicId);
    return this.http.get<Staff>(`${this.baseUrl}staff/${id}`, {params})
  }

  createStaff(payload: any): Observable<Staff> {
    return this.http.post<Staff>(`${this.baseUrl}staff`, payload);
  }

  uploadImage(formData: FormData): Observable<{url : string}> {
    return this.http.post<any>(`${this.baseUrl}staff/upload`, formData);
  }

  deleteStaff(id: number) {
    return this.http.delete<Staff>(`${this.baseUrl}staff/delete/${id}`);
  }
}
