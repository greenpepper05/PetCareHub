import { Component, inject, OnInit } from '@angular/core';
import { MatDialogRef, MatDialogClose, MatDialogTitle, MatDialogContent } from '@angular/material/dialog';
import { StaffService } from '../../../core/services/staff.service';
import { Staff } from '../../models/staff';
import { AccountService } from '../../../core/services/account.service';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-select-staff',
  imports: [ 
    MatDialogTitle, 
    MatDialogContent,
    MatIcon
    
  ],
  templateUrl: './select-staff.component.html',
  styleUrl: './select-staff.component.scss'
})
export class SelectStaffComponent implements OnInit{
  private accountService = inject(AccountService);
  private dialogRef = inject(MatDialogRef<SelectStaffComponent>);
  private staffService = inject(StaffService);

  staff: Staff[] = [];
  selectedStaffId?: number;
  clinicId!: number;

  ngOnInit(): void {
    const user = this.accountService.currentUser()?.clinicId;
    if (user) {
      this.clinicId = user;
    }
    this.loadStaff();
  }

  loadStaff() {

    this.staffService.getAllStaff(this.clinicId).subscribe({
      next: (data) => (this.staff = data)
    })
  }

  selectStaff() {
    if (this.selectedStaffId) {
      this.dialogRef.close(this.selectedStaffId);
    }
  }

  cancel() {
    this.dialogRef.close(null);
  }
}
