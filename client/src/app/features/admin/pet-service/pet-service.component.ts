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
import { MatCalendar, MatDatepicker, MatDatepickerModule } from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { environment } from '../../../../environments/environment.development';
import { ServiceRecordService } from '../../../core/services/service-record.service';
import { ServiceRecord } from '../../../shared/models/serviceRecord';
// import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


@Component({
  selector: 'app-pet-service',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [
    DatePipe,
    MatButton,
    ReactiveFormsModule,
    RouterLink,
    MatDatepickerModule
  ],
  templateUrl: './pet-service.component.html',
  styleUrl: './pet-service.component.scss',
})
export class PetServiceComponent implements OnInit {
  private fb = inject(FormBuilder);
  private services = inject(ServicesService);
  private service = inject(PetServiceHistoryService);
  private router = inject(Router);
  private historyService = inject(PetServiceHistoryService);
  private serviceRecord = inject(ServiceRecordService);
  private petService = inject(PetService);
  records: ServiceRecord[] = [];

  pets: Pet[] = [];
  serviceList: Services[] = [];
  showForm = false;
  selectedDate: Date = new Date();

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
    // this.loadHistory();
    this.fetchHistoriesByDate(new Date());
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
    this.serviceRecord.getAll().subscribe({
      next: (data) => {
        this.records = data;
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
  
  toggleForm() {
    this.showForm = !this.showForm;
    if (!this.showForm) {
      this.form.reset({
        visitType: '0'
      });
    }
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
      
      this.serviceRecord.createServiceRecord(payload).subscribe({
        next: () => {
          alert("Service added");
          // this.loadHistory();
        }
        ,
        error: err => console.error('Failed to create history:', err)
      });
      
    }
  }

  fetchHistoriesByDate(date: Date) {
    this.serviceRecord.getServiceRecordByDateAndClinicId(date).subscribe({
      next: data => {
        this.records = data;
      }
    })
  }
  
  onDateChange(date: Date) {
    this.selectedDate = date;
    this.fetchHistoriesByDate(date);
  }

}
