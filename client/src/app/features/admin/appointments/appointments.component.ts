import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, model, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatCalendar, MatDatepicker, MatDatepickerModule } from "@angular/material/datepicker";
import { AppointmentService } from '../../../core/services/appointment.service';
import { Appointment } from '../../../shared/models/appointment';

@Component({
  selector: 'app-appointments',
  providers: [provideNativeDateAdapter()],
  imports: [
    CommonModule,
    MatDatepickerModule,
    MatCardModule,
    DatePipe
],
  templateUrl: './appointments.component.html',
  styleUrl: './appointments.component.scss'
})
export class AppointmentsComponent implements OnInit{
  selected = model<Date | null>(null);
  private appointmentService = inject(AppointmentService);
  appointments: Appointment[] = [];

  ngOnInit(): void {
    this.loadAppointment();
  }

  loadAppointment() {
    this.appointmentService.getAppointmentByClinic().subscribe({
      next: appointments => {
        console.log(appointments);
        this.appointments = appointments;
      }
    })
  }
}
