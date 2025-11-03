import { DecimalPipe, NgClass } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { ClinicService } from '../../core/services/clinic.service';
import { Clinic } from '../../shared/models/clinic';
import { RouterLink } from "@angular/router";


@Component({
  selector: 'app-vets',
  imports: [
    MatIconModule,
    RouterLink,
    DecimalPipe,
    RouterLink
],
  templateUrl: './vets.component.html',
  styleUrl: './vets.component.scss'
})
export class VetsComponent implements OnInit{
  private clinicService = inject(ClinicService);
  clinics: Clinic[] = [];

  ngOnInit(): void {
    this.loadClinic();
  }

  loadClinic(){
    this.clinicService.getAllClinic().subscribe({
      next: response => {
        this.clinics = response
      }
    })
  }

  // clinics: Clinic[] = [
  //     { id: 'c1', name: 'Pawsionate Hands Vet Clinic', address: '123 Pet Street, Tarlac City', vetsCount: 5, patientsCount: 150, rating: 4.8, status: 'Active' },
  //     { id: 'c2', name: 'The Cozy Vet Clinic', address: '456 Main Ave, Quezon City', vetsCount: 3, patientsCount: 85, rating: 4.5, status: 'Active' },
  //     { id: 'c3', name: 'Metro Animal Hospital', address: '789 Central Blvd, Makati', vetsCount: 8, patientsCount: 220, rating: 4.9, status: 'Active' },
  //     { id: 'c4', name: 'Bark & Meow Wellness Center', address: '101 Side Lane, Baguio', vetsCount: 2, patientsCount: 40, rating: 4.2, status: 'Pending' },
  //     { id: 'c5', name: 'Happy Tails Surgery', address: '202 Oak Dr, Cebu', vetsCount: 6, patientsCount: 180, rating: 3.9, status: 'Active' },
  //     { id: 'c6', name: 'Urban Veterinary Care', address: '303 River Road, Davao', vetsCount: 1, patientsCount: 25, rating: 4.0, status: 'Inactive' },
  // ];

  // getStatusClasses(status: Clinic['status']): string {
  //   switch (status) {
  //     case 'Active':
  //       return 'bg-green-100 text-green-800';
  //     case 'Pending':
  //       return 'bg-yellow-100 text-yellow-800';
  //     case 'Inactive':
  //       return 'bg-red-100 text-red-800';
  //     default:
  //       return 'bg-gray-100 text-gray-800';
  //   }
  // }
}
