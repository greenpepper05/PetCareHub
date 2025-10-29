import { NgClass } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { CompletionChartComponent } from '../../../shared/components/completion-chart/completion-chart.component';
import { SignupChartComponent } from '../../../shared/components/signup-chart/signup-chart.component';
import { AccountService } from '../../../core/services/account.service';
import { PetService } from '../../../core/services/pet.service';
import { ClinicService } from '../../../core/services/clinic.service';
import { AppointmentService } from '../../../core/services/appointment.service';

@Component({
  selector: 'app-superadmin-dashboard',
  imports: [
    MatIcon,
    CompletionChartComponent,
    SignupChartComponent,
    NgClass
  ],
  templateUrl: './superadmin-dashboard.component.html',
  styleUrl: './superadmin-dashboard.component.scss'
})
export class SuperadminDashboardComponent implements OnInit{
  private accountService = inject(AccountService);
  private petService = inject(PetService);
  private clinicService = inject(ClinicService);
  private appointmentService = inject(AppointmentService);

  totalUsersCount = signal(0); 
  totalPetsCount = signal(0);
  activeClinicsCount = signal(0);

  ngOnInit(): void {
    this.loadUserCount();
    this.loadPetCount();
    this.loadClinicCount();
    this.loadAppointmentCount();
  }

  loadUserCount() {
    this.accountService.getAllUsers().subscribe({
      next: users => {
        this.totalUsersCount.set(users.length - 1)
      }
    })
  }

  loadPetCount() {
    this.petService.getAllPets().subscribe({
      next: pets => {
        this.totalPetsCount.set(pets.length);
      }
    })
  }

  loadClinicCount() {
    this.clinicService.getAllClinic().subscribe({
      next: clinics => {
        this.activeClinicsCount.set(clinics.length);
      }
    })
  }

  loadAppointmentCount() {
    this.appointmentService.getAllAppointment().subscribe({
      next: appointments => {
        this.totalAppointments.set(appointments.length);
      }
    })
    this.appointmentService.getAllConfirmedAppointment().subscribe({
      next: confirmed => {
        this.confirmedAppointmentsPercentage.set(confirmed.length)
      }
    })
  }

  metricCards = computed(() => [
    { 
      title: 'Total Users', 
      value: this.totalUsersCount(), 
      iconName: 'group', 
      iconBg: 'bg-blue-100 text-blue-600' 
    },
    { 
      title: 'Total Pets', 
      value: this.totalPetsCount(), 
      iconName: 'pets', 
      iconBg: 'bg-green-100 text-green-600' 
    },
    { 
      title: 'Active Clinics', 
      value: this.activeClinicsCount(), 
      iconName: 'apartment', 
      iconBg: 'bg-purple-100 text-purple-600' 
    }
  ]);

  confirmedAppointmentsPercentage = signal(0); 
  totalAppointments = signal(0);
  completedAppointments = computed(() => 
    Math.round((this.totalAppointments() * (this.confirmedAppointmentsPercentage() / 100)) * 100)
  );

  monthlySignups = signal([
      { month: 'January', users: 0 },
      { month: 'February', users: 0 },
      { month: 'March', users: 0 },
      { month: 'April', users: 0 },
      { month: 'May', users: 0 },
      { month: 'June', users: 1 },
      { month: 'July', users: 2 },
      { month: 'August', users: 3 },
      { month: 'September', users: 1 },
      { month: 'October', users: 5 },
  ]);
}
