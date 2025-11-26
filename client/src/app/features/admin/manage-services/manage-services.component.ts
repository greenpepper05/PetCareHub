import { Component, inject, OnInit } from '@angular/core';
import { ServicesService } from '../../../core/services/services.service';
import { Services } from '../../../shared/models/services';
import { Router, RouterLink } from '@angular/router';
import { AddServiceModalComponent } from '../../../shared/components/add-service-modal/add-service-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { ConfimationDialogComponent } from '../../../shared/components/confimation-dialog/confimation-dialog.component';

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
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);
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

  deleteProcedure(id: number) {
    const dialogRef = this.dialog.open(ConfimationDialogComponent, {
        width: '350px',
        data: {
            title: 'Confirm Deletion',
            message: 'Are you sure you want to permanently delete this service? This action cannot be undone.'
        }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.servicesService.deleteService(id).subscribe({
          next: () => {
            this.snackbarService.success("Service deleted successfully");
            this.router.navigateByUrl('admin/manage-services');
            this.loadServices();
          }
        })
      } else {
        this.snackbarService.error("Cancelled");
      }
    })

    
  }
}
