import { Component, inject, OnInit } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { StaffService } from '../../../../core/services/staff.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Staff } from '../../../../shared/models/staff';
import { AccountService } from '../../../../core/services/account.service';
import { ServiceRecord } from '../../../../shared/models/serviceRecord';
import { ServiceRecordService } from '../../../../core/services/service-record.service';
import { DatePipe, NgClass } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { SnackbarService } from '../../../../core/services/snackbar.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfimationDialogComponent } from '../../../../shared/components/confimation-dialog/confimation-dialog.component';

@Component({
  selector: 'app-staff-detail',
  imports: [
    MatIcon,
    RouterLink,
    NgClass,
    DatePipe,
    MatButton
  ],
  templateUrl: './staff-detail.component.html',
  styleUrl: './staff-detail.component.scss'
})
export class StaffDetailComponent implements OnInit{
  private router = inject(Router);
  private dialog = inject(MatDialog);
  private accountService = inject(AccountService);
  private staffService = inject(StaffService);
  private snackService = inject(SnackbarService);
  private activatedRoute = inject(ActivatedRoute);
  private serviceRecordService = inject(ServiceRecordService);
  staff?: Staff;
  serviceRecords: ServiceRecord[] = [];

  ngOnInit(): void {
    this.loadStaff();
  }

  loadStaff() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;

    const clinicId = this.accountService.currentUser()?.clinicId;
    if (!clinicId) return;

    this.staffService.getStaffById(+id, clinicId).subscribe({
      next: response => {
        this.staff = response;
        this.loadServiceRecord(+id, clinicId);
      }
    })
  }

  loadServiceRecord(id: number, clinicId: number) {
    this.serviceRecordService.getStaffServiceRecord(id, clinicId).subscribe({
      next: (records) => {
        this.serviceRecords = records;
      }
    })
  }

  deleteStaff(id: number) {
    const dialogRef = this.dialog.open(ConfimationDialogComponent, {
      width: '350px',
      data: {
        title: "Confirm Deletion",
        message: 'Are you sure you want to permanently delete this staff? This action cannot be undone.'
      }
    })

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.staffService.deleteStaff(+id).subscribe({
          next: () => {
            this.snackService.success("Staff deleted");
            this.router.navigateByUrl("/admin/manage-clinic");
          }
        })
      } else {
        this.snackService.error('Cancelled');
      }
    });
    
  }
}
