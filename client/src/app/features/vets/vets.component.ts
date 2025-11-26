import { CommonModule, DecimalPipe, NgClass } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { ClinicService } from '../../core/services/clinic.service';
import { Clinic } from '../../shared/models/clinic';
import { RouterLink } from "@angular/router";


@Component({
  selector: 'app-vets',
  imports: [
    MatIconModule,
    RouterLink,
    DecimalPipe,
    CommonModule
],
  templateUrl: './vets.component.html',
  styleUrl: './vets.component.scss'
})
export class VetsComponent implements OnInit{
  private clinicService = inject(ClinicService);
  clinics: Clinic[] = [];
  isOpen?: boolean;

  ngOnInit(): void {
    this.loadClinic();
  }

  loadClinic(){
    this.clinicService.getActiveClinic().subscribe({
      next: clinics => {
        this.clinics = clinics;

        this.clinics.forEach(c => {
          this.clinicService.getClinicStatus(c.id).subscribe(status => {
            c.isOpen = status;
          })
        })
      }
    })
  }

  getClinicStatus() {
    
  }
}
