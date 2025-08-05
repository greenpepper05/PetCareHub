import { Component, inject, OnInit } from '@angular/core';
import { PetServiceHistory } from '../../../shared/models/petServiceHistory';
import { PetServiceHistoryService } from '../../../core/services/pet-service-history.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { PetService } from '../../../core/services/pet.service';
import { Pet } from '../../../shared/models/pet';
import { DatePipe } from '@angular/common';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-pet-service',
  imports: [
    RouterLink,
    DatePipe,
    MatButton
  ],
  templateUrl: './pet-service.component.html',
  styleUrl: './pet-service.component.scss'
})
export class PetServiceComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private petService = inject(PetService);
  private router = inject(Router);
  private historyService = inject(PetServiceHistoryService);
  histories: PetServiceHistory[] = [];
  pets: Pet[] = [];
  
  ngOnInit(): void {
    this.loadHistory();
  }

  loadHistory() {
    this.historyService.getAll().subscribe({
      next: (data) => {
        this.histories = data;
      }
    })
  }

}
