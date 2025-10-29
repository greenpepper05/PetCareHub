import { Component, inject, OnInit } from '@angular/core';
import { Clinic } from '../../../shared/models/clinic';
import { NgClass } from '@angular/common';
import { ClinicService } from '../../../core/services/clinic.service';
import { MatDialog } from '@angular/material/dialog';
import { AddNewClinicComponent } from '../../../shared/components/add-new-clinic/add-new-clinic.component';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-superadmin-clinic',
  imports: [
    RouterLink
],
  templateUrl: './superadmin-clinic.component.html',
  styleUrl: './superadmin-clinic.component.scss'
})
export class SuperadminClinicComponent implements OnInit {
  private clinicService = inject(ClinicService);
  private dialog = inject(MatDialog);
  clinics: Clinic[] = [];

  ngOnInit(): void {
    this.loadClinics();
  }

  loadClinics() {
    this.clinicService.getAllClinic().subscribe({
      next: response => this.clinics = response
    })
  }

  openClinicRegistrationModal() {
    const dialogRef = this.dialog.open(AddNewClinicComponent, {
          width: '450px',
          panelClass: 'custom-dialog-container',
        });
    
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadClinics();
      }
    });
  }
  
}
