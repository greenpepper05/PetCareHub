import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PetService } from '../../../../core/services/pet.service';
import { PetServiceHistoryService } from '../../../../core/services/pet-service-history.service';
import { PetServiceHistory } from '../../../../shared/models/petServiceHistory';
import { DatePipe } from '@angular/common';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-pet-service-detail',
  imports: [
    RouterLink,
    MatButton
  ],
  templateUrl: './pet-service-detail.component.html',
  styleUrl: './pet-service-detail.component.scss'
})
export class PetServiceDetailComponent implements OnInit{
  private activatedRoute = inject(ActivatedRoute);
  private petServiceHistory = inject(PetServiceHistoryService);
  petService?: PetServiceHistory;

  ngOnInit(): void {
    this.loadPetHistory();
  }

  loadPetHistory() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;

    this.petServiceHistory.getPetHistoryByPetId(+id).subscribe({
      next: petServices => {
        this.petService = petServices;
      } 
    })
  }
}
