import { Component, inject, OnInit } from '@angular/core';
import { AppointmentService } from '../../../core/services/appointment.service';
import { Appointment } from '../../../shared/models/appointment';

@Component({
  selector: 'app-pets',
  imports: [],
  templateUrl: './pets.component.html',
  styleUrl: './pets.component.scss'
})
export class PetsComponent implements OnInit {
  private pet = inject(AppointmentService);
  pets: Appointment[] = [];

  ngOnInit(): void {
    this.pet.getAppointmentByClinic().subscribe({
      next: pets => {
        this.pets = pets;
      }
    });
  }
}
