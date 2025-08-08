import { Component, inject, OnInit } from '@angular/core';
import { PetServiceHistory } from '../../../shared/models/petServiceHistory';
import { PetServiceHistoryService } from '../../../core/services/pet-service-history.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { PetService } from '../../../core/services/pet.service';
import { Pet } from '../../../shared/models/pet';
import { DatePipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { ServicesService } from '../../../core/services/services.service';
import { Services } from '../../../shared/models/services';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-pet-service',
  standalone: true,
  imports: [
    DatePipe,
    MatButton,
    ReactiveFormsModule,
  ],
  templateUrl: './pet-service.component.html',
  styleUrl: './pet-service.component.scss'
})
export class PetServiceComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private fb = inject(FormBuilder);
  private services = inject(ServicesService);
  private service = inject(PetServiceHistoryService);
  private router = inject(Router);
  private historyService = inject(PetServiceHistoryService);
  private petService = inject(PetService);
  histories: PetServiceHistory[] = [];
  pets: Pet[] = [];
  serviceList: Services[] = [];

  form = this.fb.group({
    ownerId: [''],
    petId: [''],
    serviceId: [''],
    clinicId: 1,
    dateOfService: [''],
    notes: [''],
    visitType: [''],
    appointmentId: ['']
  });
  
  ngOnInit(): void {
    this.loadHistory();
    this.loadServices();
    this.loadPets();
    this.form.get('petId')?.valueChanges.subscribe((petId) => {
      const selectedPet = this.pets.find(p => p.id == petId);
      if (selectedPet) {
        this.form.patchValue({ownerId: selectedPet.ownerId});
      } else {
        this.form.patchValue({ ownerId: '' });
      }
    })
  }

  loadPets() {
  this.petService.getPetByClinic().subscribe({
    next: pets => this.pets = pets
    });
  }

  loadHistory() {
    this.historyService.getAll().subscribe({
      next: (data) => {
        this.histories = data;
      }
    })
  }

  loadServices() {
    this.services.getServices().subscribe({
      next: (services) => {
        this.serviceList = services
      }
    })
  }

  onSubmit() {
    if (this.form.valid) {

      const payload = {
        petId: this.form.value.petId,
        ownerId: this.form.value.ownerId,
        clinicId: 1,
        serviceId: this.form.value.serviceId,
        dateOfService: this.form.value.dateOfService,
        notes:this.form.value.notes
      };
      
      console.log(payload);
      this.service.createPetServiceHistory(payload).subscribe({
        next: (response : PetServiceHistory) => 
          
          this.router.navigate(['/admin/pet-service/' + response.id])
        
        ,
        error: err => console.error('Failed to create history:', err)
      });

      this.loadHistory();
    }
  }

}
