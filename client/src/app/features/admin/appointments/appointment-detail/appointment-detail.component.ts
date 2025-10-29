import { Component, inject, OnInit } from '@angular/core';
import { AppointmentService } from '../../../../core/services/appointment.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Appointment } from '../../../../shared/models/appointment';
import { MatButton } from '@angular/material/button';
import { DatePipe } from '@angular/common';
import { MatFormField, MatLabel, MatOption, MatSelect } from '@angular/material/select';
import { Services } from '../../../../shared/models/services';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment.development';
import { MatIcon } from '@angular/material/icon';

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
    MatFormField,
    MatIcon
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
  services?: Services;

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
        // this.services = appointment;

      } 
    })
  }

  saveStatus() {
    const selectedStatus = this.statusForm.get('status')?.value;
    
    const id = this.appointment?.id;
    console.log(id);

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

  
}
