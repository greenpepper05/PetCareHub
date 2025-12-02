import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTabsModule } from '@angular/material/tabs';
import { ClinicService } from '../../../core/services/clinic.service';
import { ClinicSchedule } from '../../../shared/models/clinicSchedule';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { AccountService } from '../../../core/services/account.service';
import { SaveScheduleDto } from '../../../shared/models/saveScheduleDto';
import { StaffService } from '../../../core/services/staff.service';
import { Staff } from '../../../shared/models/staff';
import { MatDialog } from '@angular/material/dialog';
import { CreateStaffComponent } from '../../../shared/components/create-staff/create-staff.component';
import { RouterLink } from "@angular/router";


@Component({
  selector: 'app-manage-clinic',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButton,
    MatIcon,
    MatSelectModule,
    MatSlideToggleModule,
    MatTabsModule,
    RouterLink
],
  templateUrl: './manage-clinic.component.html',
  styleUrl: './manage-clinic.component.scss'
})
export class ManageClinicComponent implements OnInit {
  private accountService = inject(AccountService);
  private clinicService = inject(ClinicService);
  private staffService = inject(StaffService);
  private snackbarService = inject(SnackbarService);
  private dialog = inject(MatDialog);
  schedules: ClinicSchedule[] = [];
  staff: Staff[] =[];
  clinicId!: number;


  daysOfWeek = [
      'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'
    ];

  scheduleForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    const user = this.accountService.currentUser();
    if (user) {
      this.clinicId = user.clinicId;
      this.initializeForm();
      this.loadSchedule();
      this.loadStaff();
    }
  }

  initializeForm() {
    this.scheduleForm = this.fb.group({
      days: this.fb.array(this.daysOfWeek.map(day => this.createDayGroup(day)))
    });
  }

  createDayGroup(day: string): FormGroup {
    return this.fb.group({
      day: [day],
      opening: [''],
      closing: ['']
    });
  }

  // convenience getter
  get days(): FormArray {
    return this.scheduleForm.get('days') as FormArray;
  }

  getDayGroup(index: number): FormGroup {
    return this.days.at(index) as FormGroup;
  }

  private toBackendTime(hhmm: string | undefined | null): string {
    if (!hhmm) return ''; 
    if (hhmm.length === 8 && hhmm[2] === ':') return hhmm;
    if (hhmm.length === 5 && hhmm[2] === ':') return `${hhmm}:00`;
    return hhmm;
  }

  isEditing = false;

  toggleEdit() {
    this.isEditing = !this.isEditing;
    this.initializeForm();
  }

  formatTime(time: string | null): string {
    if (!time) return '';
    const [h, m] = time.split(':');
    const hour = Number(h);
    const ampm = hour >= 12 ? 'PM' : 'AM';
    const displayHour = hour % 12 || 12;
    return `${displayHour}:${m} ${ampm}`;
  }

  getDayName(day: number): string {
    return [
      'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'
    ][day];
  }


  onSave() {

    if (this.scheduleForm.invalid) return;

    const schedules: SaveScheduleDto[] = this.days.controls.map((ctrl, index) => {
      const fg = ctrl as FormGroup;
      const val = fg.value;

      const openingRaw: string = val.opening?.trim();
      const closingRaw: string = val.closing?.trim();

      const openingTime = this.toBackendTime(openingRaw) || '09:00:00';
      const closingTime = this.toBackendTime(closingRaw) || '17:00:00';

      const isOpen = !!(openingRaw || closingRaw);

      return {
        clinicId: this.clinicId,
        dayOfWeek: index,
        isOpen,
        openingTime,
        closingTime
      } as SaveScheduleDto;
    });

    this.clinicService.saveClinicSchedules(this.clinicId, schedules).subscribe({
      next: () => {
        this.snackbarService.success("Schedule successfully saved!");
        this.loadSchedule();
        this.isEditing = false;
      }
    });

  }

  loadSchedule() {
    const clinicId = this.accountService.currentUser()?.clinicId;

    if (!clinicId) return;

    this.clinicService.getClinicSchedules(clinicId).subscribe({
      next: (data) => {
        this.schedules = data;
      }
    });
  }

  loadStaff() {

    this.staffService.getAllStaff(this.clinicId).subscribe({
      next: res => this.staff = res
    })
  }

  openCreateStaffModal() {
    const dialogRef = this.dialog.open(CreateStaffComponent, {
      width: '750px',
      maxWidth: '90vw',
      disableClose: true,
      autoFocus: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) this.loadStaff();
    });
  }

}
