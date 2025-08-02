import { DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AppointmentService } from '../../core/services/appointment.service';
import { Appointment } from '../../shared/models/appointment';

@Component({
  selector: 'app-myappointments',
  imports: [
    RouterLink,
    DatePipe
  ],
  templateUrl: './myappointments.component.html',
  styleUrl: './myappointments.component.scss'
})
export class MyappointmentsComponent implements OnInit {
  private appointmentService = inject(AppointmentService);
  appointments: Appointment[] = [];

  ngOnInit(): void {
    this.appointmentService.getAppointmentForUser().subscribe({
      next: appointments => {
        console.log(appointments); 
        this.appointments = appointments;
      } 
    })
  }

}
