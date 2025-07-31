import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatCard } from "@angular/material/card";
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { AppointmentService } from '../../core/services/appointment.service';
import { Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOption, provideNativeDateAdapter} from '@angular/material/core';
import { PetService } from '../../core/services/pet.service';
import { ServicesService } from '../../core/services/services.service';
import { MatRadioModule} from '@angular/material/radio';
import { MatStepperModule} from '@angular/material/stepper';
import { MatSelectModule} from '@angular/material/select';
import { Pet } from '../../shared/models/pet';
import { AccountService } from '../../core/services/account.service';

@Component({
  selector: 'app-appointment',
  providers: [provideNativeDateAdapter()],
  imports: [
    ReactiveFormsModule,
    MatFormField,
    MatLabel,
    MatInputModule,
    MatDatepickerModule,
    MatRadioModule,
    MatStepperModule,
    MatOption,
    MatSelectModule
],
  templateUrl: './appointment.component.html',
  styleUrl: './appointment.component.scss'
})
export class AppointmentComponent implements OnInit{
  private fb = inject(FormBuilder);
  private appointmentService = inject(AppointmentService);
  private petService = inject(PetService);
  private servicesService = inject(ServicesService);
  private accountSerive = inject(AccountService);
  private router = inject(Router);
  services: any[] = [];
  
  ngOnInit(): void {
      this.servicesService.getServices().subscribe({
      next: response => this.services = response,
      error: error => console.log(error)
    })
  }

  appointmentForm = this.fb.group({
    appoisntmentDate: [''],
    petid: [''],
    ownerid: [''],
    serviceid: [''],
    notes: [''],
    clinicid: [''],
  });

  petProfileForm = this.fb.group({
    id: [''],
    name: [''],
    breed: [''],
    species: [''],
    birthdate: [''],
    gender: [''],
    ownerid: ['']
  })

  servicesForm = this.fb.group({
    serviceId: ['']
  })

  onSubmit() {
    const currentUser = this.accountSerive.currentUser();

    if (!currentUser) {
      console.error('No logged-in user found,');
      return;
    }

    const ownerid = currentUser.id;

    this.petProfileForm.patchValue({ ownerid});

    console.log(this.petProfileForm.value)
    this.petService.createPetProfile(this.petProfileForm.value).subscribe({
      next: (pet : Pet) => {
        const selectedService = this.servicesForm.get('serviceId')?.value;

        this.appointmentForm.patchValue({ 
          petid: pet.id,
          ownerid: pet.ownerid,
          serviceid: selectedService
        });
        
        this.appointmentForm.patchValue({ serviceid: selectedService});

        const payload = {
          serviceId: Number(this.servicesForm.value.serviceId),
          ownerId: this.accountSerive.currentUser()?.id ?? '',
          petId: Number(this.petProfileForm.value.id),
          appointmentDate : this.appointmentForm.value.appoisntmentDate 
        }
        this.appointmentService.createAppointment(payload).subscribe({
          next: () => this.router.navigate(['appointments/appointment-confirmation']),
          error: error => console.error('Appointment error', error)
        })
      },
      error: error => console.error('Pet creation error', error)
    })
  }

}
