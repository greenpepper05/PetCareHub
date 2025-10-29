import { Component, inject, OnInit } from '@angular/core';
import { Pet } from '../../../shared/models/pet';
import { PetService } from '../../../core/services/pet.service';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AccountService } from '../../../core/services/account.service';
import { Pagination } from '../../../shared/models/pagination';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { PetParams } from '../../../shared/models/petParams';
import { MatInput, MatInputModule } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-pets',
  imports: [
    DatePipe,
    RouterLink,
    MatPaginator,
    MatInputModule,
    MatIcon
],
  templateUrl: './pets.component.html',
  styleUrl: './pets.component.scss'
})
export class PetsComponent implements OnInit {
  private pet = inject(PetService);
  private accountService = inject(AccountService);
  petsPagination?: Pagination<Pet>;
  petParams = new PetParams();
  pets: Pet[] = [];

  totalCount = 0;
  pageSizeOptions = [10,20,30];
  
  ngOnInit(): void {
    this.loadPet();
    
  }

  async loadPet() {
    const currentUser = this.accountService.currentUser();

    if (!currentUser) {
      return;
    }

    const clinicId = currentUser.clinicId
    
    this.pet.getAllPetByClinic(this.petParams, clinicId).subscribe({
      next: data => {
        this.petsPagination = data;
        this.totalCount = data.count;
      }
    });
  }

  handlePageEvent(event: PageEvent) {
    this.petParams.pageNumber = event.pageIndex + 1;
    this.petParams.pageSize = event.pageSize;
    this.loadPet();
  }

  onSearchChange() {
    // this.petParams.pageSize = 1;
    // this.loadPet();
  }

}