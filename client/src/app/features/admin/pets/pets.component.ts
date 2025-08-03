import { Component, inject, OnInit } from '@angular/core';
import { AppointmentService } from '../../../core/services/appointment.service';
import { Appointment } from '../../../shared/models/appointment';
import { Pet } from '../../../shared/models/pet';
import { PetService } from '../../../core/services/pet.service';

@Component({
  selector: 'app-pets',
  imports: [],
  templateUrl: './pets.component.html',
  styleUrl: './pets.component.scss'
})
export class PetsComponent implements OnInit {
  private pet = inject(PetService);
  pets: Pet[] = [];

  ngOnInit(): void {
    this.pet.getPetByClinic().subscribe({
      next: pets => {
        this.pets = pets;
      }
    });
  }
}
