import { CurrencyPipe, DatePipe, DecimalPipe, NgClass } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { PetService } from '../../../core/services/pet.service';
import { AccountService } from '../../../core/services/account.service';
import { ServiceRecordService } from '../../../core/services/service-record.service';
import { ServiceRecord } from '../../../shared/models/serviceRecord';
import { AppointmentService } from '../../../core/services/appointment.service';
import { AppointmentParams } from '../../../shared/models/appointmentParams';
import { Appointment } from '../../../shared/models/appointment';
import { Pagination } from '../../../shared/models/pagination';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MatIcon,
    DecimalPipe,
    CurrencyPipe,
    NgClass,
    DatePipe,
    RouterLink
],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit{
  private petService = inject(PetService);
  private accountService = inject(AccountService);
  private serviceRecordService = inject(ServiceRecordService);
  private appointmentService = inject(AppointmentService);
  
  totalPatients = signal(0);
  totalRevue = signal(0);
  totalAppointments = signal(0);

  upcoming?: Pagination<Appointment>;

  appointmentParams = new AppointmentParams();

  clinicId = this.accountService.currentUser()?.clinicId;

  ngOnInit(): void {
    this.loadPetCount();
    this.loadRevenueCounter();
    this.loadAppointmentCount();
    this.loadUpcoming();
  }

  loadPetCount() {
    this.accountService.getUserInfo().subscribe({
      next: user => {
        this.petService.getPetByClinic(user.clinicId).subscribe({
          next: pets => {
        this.totalPatients.set(pets.length)
      }
        })
      }
    })
  }

  loadRevenueCounter() {
    this.serviceRecordService.getAllServiceRecord().subscribe({
      next: (rev: ServiceRecord[]) => {
        const total = rev.reduce((sum, record) => sum + record.price!, 0)

        this.totalRevue.set(total);
      }
    })
  }

  loadAppointmentCount() {
    this.accountService.getUserInfo().subscribe({
      next: user => {
        this.appointmentService.getAllAppointmentByClinicId(user.clinicId).subscribe({
          next: appointment => {
            this.totalAppointments.set(appointment.length)
          }
        })

      }
    })
  }

  loadUpcoming() {

    if (!this.clinicId)  return;

    this.appointmentService.getUpcomingAppointment(this.appointmentParams, this.clinicId).subscribe({
      next: (data) => this.upcoming = data
    })
  }

  upcomingAppointments() {
    return this.upcoming?.data ?? [];
  }

}
