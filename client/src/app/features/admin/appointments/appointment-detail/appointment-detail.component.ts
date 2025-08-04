import { Component, inject, OnInit } from '@angular/core';
import { AppointmentService } from '../../../../core/services/appointment.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Appointment } from '../../../../shared/models/appointment';
import { MatButton } from '@angular/material/button';
import { DatePipe } from '@angular/common';
import { MatFormField, MatLabel, MatOption, MatSelect } from '@angular/material/select';
import { Service } from '../../../../shared/models/services';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment.development';

@Component({
  selector: 'app-appointment-detail',
  imports: [
    ReactiveFormsModule,
    MatButton,
    RouterLink,
    DatePipe,
    MatSelect,
    MatOption,
    MatLabel,
    MatFormField
],
  templateUrl: './appointment-detail.component.html',
  styleUrl: './appointment-detail.component.scss'
})
export class AppointmentDetailComponent implements OnInit {
  baseUrl = environment.apiUrl;
  private fb = inject(FormBuilder);
  private appointmentService = inject(AppointmentService);
  private activatedRoute = inject(ActivatedRoute);
  private http = inject(HttpClient);
  appointment?: Appointment;
  services?: Service;

  ngOnInit(): void {
    this.loadAppointment();
  }

  statusForm = this.fb.group({
    status: ['']
  });

  loadAppointment() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;
    this.appointmentService.getAppointmentDetail(+id).subscribe({
      next: appointment => {
        this.appointment = appointment;
        this.services = appointment?.service;
      } 
    })
  }

  saveStatus() {
    const selectedStatus = this.statusForm.get('status')?.value;

    if (!selectedStatus) return;

    this.appointmentService.updateStatus(this.appointment?.id!, selectedStatus).subscribe({
      next: () => {
        alert('Status updated successfully!');
      },
      error: err => {
        console.error(err);
        alert("Failed to update status.");
      }
    })
  }

  
  groomingSteps = [
    'Brushing',
    'Bathing',
    'Drying',
    'Ear Cleaning',
    'Nail Trimming',
    'Coat Trimming',
    'Completed'
  ]
  currentStep: string = '';

  currentStepIndex = 0;

  getToNextStep(step: string) {
    const clickedIndex = this.groomingSteps.indexOf(step);
    if (clickedIndex !== this.currentStepIndex) return;

      this.http.post(this.baseUrl + `appointments/${this.appointment?.id}/step`, { step })
        .subscribe({
          next: () => {
            this.currentStep = step;
            this.currentStepIndex++;
          }
        });
  }
}
