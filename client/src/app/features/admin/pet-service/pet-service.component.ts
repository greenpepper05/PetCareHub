import { Component, inject, OnInit, Optional } from '@angular/core';
import { PetServiceHistoryService } from '../../../core/services/pet-service-history.service';
import { Router, RouterLink } from '@angular/router';
import { PetService } from '../../../core/services/pet.service';
import { Pet } from '../../../shared/models/pet';
import { DatePipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { ServicesService } from '../../../core/services/services.service';
import { Services } from '../../../shared/models/services';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MatOption, provideNativeDateAdapter } from '@angular/material/core';
import { ServiceRecordService } from '../../../core/services/service-record.service';
import { ServiceRecord } from '../../../shared/models/serviceRecord';
import { AccountService } from '../../../core/services/account.service';
import { MatIcon } from '@angular/material/icon';
import { MatFormFieldModule, MatHint, MatLabel } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
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
    MatDatepickerModule,
    MatIcon,
    MatLabel,
    MatFormFieldModule,
    MatNativeDateModule,
    MatSelectModule,
    MatInputModule,
    MatOption,
    MatHint
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
  private accountService = inject(AccountService);
  records: ServiceRecord[] = [];

  pets: Pet[] = [];
  serviceList: Services[] = [];
  showForm = false;
  selectedDate: Date = new Date();

  form = this.fb.group({
    ownerId: [''],
    petId: [''],
    serviceId: [''],
    clinicId: [''],
    price: [''],
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
      } else if (petId === null || petId === '') {
        this.form.patchValue({ ownerId: '' });
      }
    })
  }

  loadPets() {

    const currentUser = this.accountService.currentUser();

    if (!currentUser) return;

    const clinicId = currentUser.clinicId;

    this.petService.getPetByClinic(clinicId).subscribe({
      next: pets => this.pets = pets
      });
  }

  loadServiceRecord() {
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

      const currentUser = this.accountService.currentUser();

      if (!currentUser) return;
      
      const clinicId = currentUser.clinicId;

      const rawValue = this.form.value;
      const finalAppointmentId = rawValue.appointmentId === '' ? null : rawValue.appointmentId;
      
      const date = this.form.value.dateOfService;
      const formattedDate = date ? new Date(date).toLocaleDateString('en-CA').split('T')[0] : '';

      const payload = {
        petId: this.form.value.petId,
        ownerId: this.form.value.ownerId,
        serviceId: this.form.value.serviceId,
        clinicId: clinicId,
        price: this.form.value.price,
        dateOfService: formattedDate,
        notes:this.form.value.notes,
        visitType: this.form.value.visitType,
        appointmentId: finalAppointmentId
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
