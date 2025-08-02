import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Appointment } from '../../../shared/models/appointment';
import { AppointmentService } from '../../../core/services/appointment.service';

@Component({
  selector: 'app-appointment-success',
  standalone: true,
  imports: [
    DatePipe,
    CommonModule
  ],
  templateUrl: './appointment-success.component.html',
  styleUrl: './appointment-success.component.scss'
})
export class AppointmentSuccessComponent{
  appointment!: Appointment;
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private appointmentService = inject(AppointmentService);

  // constructor(private router: Router) {
  //   const nav = this.router.getCurrentNavigation();
  //   this.appointment = nav?.extras?.state?.['appointment'];

  //   if (!this.appointment) {
  //     console.log('No appointment data found in navigation state.');
  //   } else {
  //     console.log("Appointment loaded: ", this.appointment);
  //   }
  // }

  ngOnInit(): void {
    if (!this.appointmentService.appointmentComplete) {
      this.router.navigate(['/']);
      return;
    }''
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.appointmentService.getAppointment(id).subscribe({
      next: (data) => {
        this.appointment = data;
        console.log('Full appointment loaded: ', data);
      },
      error: (err) => {
        console.error('Error fetching appointment:', err);
        this.router.navigate(['/']);
      }
    });
  }

  ngOnDestroy(): void {
    this.appointmentService.appointmentComplete = false;
    this.appointmentService.appointmentSignal.set(null);

  }
}
