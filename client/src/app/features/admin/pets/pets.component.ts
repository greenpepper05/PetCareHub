import { Component, inject, OnInit } from '@angular/core';
import { Pet } from '../../../shared/models/pet';
import { PetService } from '../../../core/services/pet.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-pets',
  imports: [
    DatePipe,
    
  ],
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