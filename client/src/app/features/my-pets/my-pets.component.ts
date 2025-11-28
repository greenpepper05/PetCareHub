import { Component, inject, OnInit } from '@angular/core';
import { PetService } from '../../core/services/pet.service';
import { Pet } from '../../shared/models/pet';
import { DatePipe } from '@angular/common';
import { environment } from '../../../environments/environment';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-my-pets',
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './my-pets.component.html',
  styleUrl: './my-pets.component.scss'
})
export class MyPetsComponent implements OnInit{
  private petService = inject(PetService);
  pets: Pet[] = [];
  baseUrl = environment.apiUrl;

  ngOnInit(): void {
    this.petService.getPetsByOwner().subscribe({
      next: pets => {
        this.pets = pets
      }
    })
  }
}
