import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, provideNativeDateAdapter } from '@angular/material/core';
import { createPetServiceHistory } from '../../../../shared/models/createPetServiceHistory';
import { PetServiceHistoryService } from '../../../../core/services/pet-service-history.service';
import { PetService } from '../../../../core/services/pet.service';
import { Pet } from '../../../../shared/models/pet';

@Component({
  selector: 'app-create',
  providers: [provideNativeDateAdapter()],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatDatepickerModule,
    MatDatepickerModule,
  ],
  templateUrl: './create.component.html',
  styleUrl: './create.component.scss'
})
export class CreateComponent implements OnInit{
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private service = inject(PetServiceHistoryService);
  private petService = inject(PetService);
  pets: Pet[] = [];

  services = [
    { id: 1, name: 'Consultation' },
    { id: 2, name: 'Vaccination' },
    { id: 3, name: 'Deworming' },
    { id: 4, name: 'Pet Grooming' },
    { id: 5, name: 'Surgery' },
    { id: 1002, name: 'Pet Boarding' }
  ];

  form = this.fb.group({
    ownerId: [''],
    petId: [null, Validators.required],
    serviceId: [null, Validators.required],
    clinicId: 1,
    dateOfService: ['', Validators.required],
    notes: [''],
    visitType: [null, Validators.required],
    appointmentId: [null]
  });

  ngOnInit(): void { 
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


  onSubmit() {
    if (this.form.valid) {
      const raw = this.form.getRawValue();

      const payload: Partial<createPetServiceHistory> = {
        petId: raw.petId ?? undefined,
        ownerId: raw.ownerId ?? undefined,
        clinicId: 1,
        serviceId: raw.serviceId ?? undefined,
        dateOfService: raw.dateOfService ? new Date(raw.dateOfService).toISOString() : undefined,
        notes: raw.notes ?? '' // ensure notes is never null
      };
      
      console.log(payload);
      this.service.createPetServiceHistory(payload).subscribe({
        next: () => 
          
          this.router.navigate(['/admin/pet-service'])
        
        ,
        error: err => console.error('Failed to create history:', err)
      });
    }
  }
}
