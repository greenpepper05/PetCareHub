import { Component, inject, OnInit } from '@angular/core';
import { ServicesService } from '../../../core/services/services.service';
import { Services } from '../../../shared/models/services';
import { RouterLink } from '@angular/router';
import { AddServiceModalComponent } from '../../../shared/components/add-service-modal/add-service-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-manage-services',
  imports: [
    RouterLink,
    MatIcon
  ],
  templateUrl: './manage-services.component.html',
  styleUrl: './manage-services.component.scss'
})
export class ManageServicesComponent implements OnInit {
  private servicesService = inject(ServicesService);
  private dialog = inject(MatDialog);
  services: Services[] = [];

  ngOnInit(): void {
    this.loadServices();
  }

  loadServices() {
    this.servicesService.getServiceByClinicId().subscribe({
      next: response => this.services = response,
    })
  }

  openAddServiceModal(): void {
    const dialogRef = this.dialog.open(AddServiceModalComponent, {
      width: '450px',
      panelClass: 'custom-dialog-container',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadServices();
      }
    });
  }
}
