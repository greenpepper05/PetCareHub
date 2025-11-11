import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTabsModule } from '@angular/material/tabs';
import { ClinicService } from '../../../core/services/clinic.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ClinicSchedule } from '../../../shared/models/clinicSchedule';
import { SnackbarService } from '../../../core/services/snackbar.service';

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
  private fb = inject(FormBuilder);
  private clinicService = inject(ClinicService);
  private router = inject(ActivatedRoute);
  private dialogRef = inject(MatDialog);
  private snackbarService = inject(SnackbarService);

  clinicId!: number;
  scheduleForm!: FormGroup;
  clinicSchedule!: ClinicSchedule;

  daysOfWeek = [
    'Sunday','Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'
  ]

  ngOnInit(): void {
    this.initializeForm();
    this.loadClinicSchedule();
  }

  initializeForm() {
    this.scheduleForm = this.fb.group({
      schedules: this.fb.array(this.daysOfWeek.map(day =>
        this.fb.group({
          dayOfWeek: [day],
          isOpen: [false],
          openingTime: ['9:00'],
          closingTime: ['17:00']
        })
      ))
    });
  }

  get schedules(): FormArray{
    return this.scheduleForm.get('schedules') as FormArray;
  }

  loadClinicSchedule() {
    this.clinicService.getClinicSchedules(this.clinicId).subscribe({
      next: (savedSchedules) => {
        savedSchedules.forEach((saved: ClinicSchedule) =>{
          const formDay = this.schedules.controls.find(
            c => c.value.dayOfWeek === saved.dayOfWeek
          );
          if (formDay) {
            formDay.patchValue(saved);
          }
        });
      },
      error: (err) => console.error('Error loading clinic schedule')
    })
  }

  saveClinicSchedule() {
    const scheduleData = this.scheduleForm.value.schedules;

    this.clinicService.saveClinicSchedules(this.clinicId, scheduleData).subscribe({
      next: () => this.snackbarService.success("Clinic schedule saved succesfully!"),
      error: (err) => this.snackbarService.error("Error saving schedule: " + err)
    });
  }

}
