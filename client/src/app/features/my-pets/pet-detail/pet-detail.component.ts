import { Component, inject, OnInit } from '@angular/core';
import { PetService } from '../../../core/services/pet.service';
import { ServiceRecord } from '../../../shared/models/serviceRecord';
import { ServicesService } from '../../../core/services/services.service';
import { ServiceRecordService } from '../../../core/services/service-record.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CurrencyPipe, DatePipe, LowerCasePipe, SlicePipe } from '@angular/common';
import { Pet } from '../../../shared/models/pet';

@Component({
  selector: 'app-pet-detail',
  imports: [
    RouterLink,
    DatePipe
],
  templateUrl: './pet-detail.component.html',
  styleUrl: './pet-detail.component.scss'
})
export class PetDetailComponent implements OnInit{
  private serviceRecord = inject(ServiceRecordService);
  private activatedRoute = inject(ActivatedRoute);
  private petService = inject(PetService);
  records: ServiceRecord[] = [];
  pet: Pet[] = [];
  petId = 0;

  ngOnInit(): void {
    this.loadPet();
  }

  loadPet() {
    
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if (id == null) return;

    console.log(id);

    this.serviceRecord.getRecordByPetId(id).subscribe({
      next: data => {
        this.records = data;
        this.petId = +id;
      }
    })

  }



}
