import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { MatCard } from "@angular/material/card";
import { MatIcon } from '@angular/material/icon';
import { PetService } from '../../../core/services/pet.service';
import { AccountService } from '../../../core/services/account.service';
import { ServiceRecordService } from '../../../core/services/service-record.service';
import { ServiceRecord } from '../../../shared/models/serviceRecord';
import { AppointmentService } from '../../../core/services/appointment.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MatIcon,
    DecimalPipe,
    CurrencyPipe
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

  ngOnInit(): void {
    this.loadPetCount();
    this.loadRevenueCounter();
    this.loadAppointmentCount();
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

  upcomingAppointments() {
    return [];
  }

}
