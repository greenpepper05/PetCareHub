import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PetServiceHistoryService } from '../../../../core/services/pet-service-history.service';
import { PetServiceHistory } from '../../../../shared/models/petServiceHistory';
import { DatePipe } from '@angular/common';
import { PetService } from '../../../../core/services/pet.service';
import { AccountService } from '../../../../core/services/account.service';
import { Pet } from '../../../../shared/models/pet';
import { User } from '../../../../shared/models/user';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-pet-service-detail',
  imports: [
    DatePipe,
    MatButton,
    RouterLink
  ],
  templateUrl: './pet-service-detail.component.html',
  styleUrl: './pet-service-detail.component.scss'
})
export class PetServiceDetailComponent implements OnInit{
  private activatedRoute = inject(ActivatedRoute);
  private petServiceHistory = inject(PetServiceHistoryService);
  private petService = inject(PetService);
  private user = inject(AccountService);
  services?: PetServiceHistory;
  pet?: Pet;
  owner?: User;

  ngOnInit(): void {
    this.loadPetHistory();

  }

  loadPetHistory() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;

    this.petServiceHistory.getPetHistoryByPetId(+id).subscribe({
      next: (history) => {
        this.services = history;
        this.loadPet(history.petId);
        console.log(this.services);
      } 
    })
  }

  loadPet(petId: number) {
    this.petService.getPetById(petId).subscribe({
      next: (pets) => {
        this.pet = pets
        const userId = pets.ownerId
        this.loadOwner(userId);
      }
    })
  }

  loadOwner(userId: string) {
    this.user.getUserById(userId).subscribe({
      next: (userInfo) => {
        console.log(userInfo)
        if (userInfo.id === userId) {
          this.owner = userInfo;
        }
      }
    })
  }
}
