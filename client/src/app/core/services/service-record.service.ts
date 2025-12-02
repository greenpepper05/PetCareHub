import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ServiceRecord } from '../../shared/models/serviceRecord';
import { Observable } from 'rxjs';
import { ServiceRecordParams } from '../../shared/models/serviceRecordParams';
import { Pagination } from '../../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ServiceRecordService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  getServiceRecordByDateAndClinicId(date: Date) {
    const formattedDate = date.toLocaleDateString('en-CA').split('T')[0];
    return this.http.get<ServiceRecord[]>(`${this.baseUrl}servicerecord/by-date?date=${formattedDate}`);
  }

  createServiceRecord(payload: any): Observable<ServiceRecord> {
    return this.http.post<ServiceRecord>(this.baseUrl + 'servicerecord', payload);
  }

  getAll(): Observable<ServiceRecord[]> {
    return this.http.get<ServiceRecord[]>(`${this.baseUrl}servicerecord`);
  }

  getServiceRecordByPetId(id: number){
      return this.http.get<ServiceRecord>(this.baseUrl + 'servicerecord/' + id);
  }

  getPetServiceRecordById(id: number){
      return this.http.get<ServiceRecord[]>(this.baseUrl + 'servicerecord/' + id);
  }

  deleteServiceRecord(recordId: number){
    return this.http.delete<ServiceRecord>(`${this.baseUrl}servicerecord/records/${recordId}`);
  }

  getServiceRecordByPetAndClinic(serviceRecordParams: ServiceRecordParams ,petId: number, clinicId: number) {
    let params = new HttpParams()
      .set('petId', petId.toString())
      .set('clinicId', clinicId.toString())
      .set('pageSize', serviceRecordParams.pageSize)
      .set('pageIndex', serviceRecordParams.pageNumber);

    if (serviceRecordParams.sort) {
      params.append('sort', serviceRecordParams.sort);
    }

    if (serviceRecordParams.search) {
      params.append('search', serviceRecordParams.search);
    }

    // params.append('pageSize', serviceRecordParams.pageSize);
    // params.append('pageIndex', serviceRecordParams.pageNumber);
    
    return this.http.get<Pagination<ServiceRecord>>(`${this.baseUrl}servicerecord/pet`, { params });
  }

  startProcedure(serviceId: number) {
    return this.http.patch(`${this.baseUrl}servicerecord/start-procedure`, serviceId);
  }

  getAllServiceRecord() {
    return this.http.get<ServiceRecord[]>(`${this.baseUrl}servicerecord/all`);
  }

  getRecordByPetId(id: string) {
    return this.http.get<ServiceRecord[]>(`${this.baseUrl}servicerecord/pet-detail/${id}`);
  }

  assignStaff(recordId: number, staffId: number) {
    return this.http.patch(`${this.baseUrl}servicerecord/${recordId}/assign-staff`, {staffId}, { observe: 'response'})
  }

  getStaffServiceRecord(id: number, clinicId: number) {
    return this.http.get<ServiceRecord[]>(`${this.baseUrl}servicerecord/staff/service-record`, {params: {staffId: id, clinicId: clinicId}});
  }

  getRecordByPetIdAndServiceId(petId: number, serviceId: number) {
    return this.http.get<ServiceRecord>(`${this.baseUrl}servicerecord/pet-record`, {params: {petId: petId, serviceId: serviceId}});
  }

 
}
