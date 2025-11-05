import { Component, inject, OnInit } from '@angular/core';
import { AppointmentService } from '../../../core/services/appointment.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Appointment } from '../../../shared/models/appointment';
import { DatePipe, NgClass } from '@angular/common';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-myappointments-detail',
  imports: [
    DatePipe,
    MatButton,
    RouterLink,
    NgClass
  ],
  templateUrl: './myappointments-detail.component.html',
  styleUrl: './myappointments-detail.component.scss'
})
export class MyappointmentsDetailComponent implements OnInit {
  private appointmentService = inject(AppointmentService);
  private activatedRoute = inject(ActivatedRoute);
  appointment?: Appointment;

  ngOnInit(): void {
    this.loadAppointment();
  }

  loadAppointment() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;
    this.appointmentService.getAppointmentDetail(+id).subscribe({
      next: appointment => this.appointment = appointment
    })
  }

}

