import { Component, inject, OnInit } from '@angular/core';
import { PetService } from '../../../../core/services/pet.service';
import { ServiceRecordService } from '../../../../core/services/service-record.service';
import { AccountService } from '../../../../core/services/account.service';
import { Pet } from '../../../../shared/models/pet';
import { User } from '../../../../shared/models/user';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ServiceRecord } from '../../../../shared/models/serviceRecord';
import { CommonModule, DatePipe } from '@angular/common';
import { ServiceRecordParams } from '../../../../shared/models/serviceRecordParams';
import { Pagination } from '../../../../shared/models/pagination';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-pet-details',
  imports: [
    DatePipe,
    RouterLink,
    CommonModule,
    MatPaginator,
    MatIcon
  ],
  templateUrl: './pet-details.component.html',
  styleUrl: './pet-details.component.scss'
})
export class PetDetailsComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private petService = inject(PetService);
  private serviceRecord = inject(ServiceRecordService);
  private ownerService = inject(AccountService);
  pet?: Pet;
  owner?: User;
  services?: ServiceRecord[] = [];
  servicesPagination?: Pagination<ServiceRecord>; 
  serviceRecordParams = new ServiceRecordParams();
  totalCount = 0;
  pageSizeOptions = [10,20,30];

  ngOnInit(): void {
    this.loadPet();

  }

  loadPetServiceRecord(petId: number, clinicId: number): void {

    this.serviceRecord.getServiceRecordByPetAndClinic(this.serviceRecordParams,petId, clinicId).subscribe({
      next: data => { 
        this.servicesPagination = data;
        this.totalCount = data.count
      } 
    });
  }

  handlePageEvent(event: PageEvent) {
    this.serviceRecordParams.pageNumber = event.pageIndex + 1;
    this.serviceRecordParams.pageSize = event.pageSize;
    this.loadPet();
  }

  loadPet() {

    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if (!id) return;

    const currentUser = this.ownerService.currentUser();

    if (!currentUser) return;

    const clinicId = currentUser.clinicId;

    this.petService.getPetById(+id).subscribe({
      next: (pets) => {
        this.pet = pets
        const userId = pets.ownerId
        this.loadOwner(userId);
        this.loadPetServiceRecord(parseInt(id), clinicId)
      }
    })
  }

  loadOwner(userId: string) {
    this.ownerService.getUserById(userId).subscribe({
      next: (userInfo) => {
        console.log(userInfo)
        if (userInfo.id === userId) {
          this.owner = userInfo;
        }
      }
    })
  }


}
