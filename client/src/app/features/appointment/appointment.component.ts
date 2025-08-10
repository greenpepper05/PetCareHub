import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatFormField, MatHint, MatLabel } from '@angular/material/form-field';
import { AppointmentService } from '../../core/services/appointment.service';
import { Router, RouterLink } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { provideNativeDateAdapter} from '@angular/material/core';
import { PetService } from '../../core/services/pet.service';
import { ServicesService } from '../../core/services/services.service';
import { MatRadioModule} from '@angular/material/radio';
import { MatStepperModule} from '@angular/material/stepper';
import { MatSelectModule} from '@angular/material/select';
import { Pet } from '../../shared/models/pet';
import { AccountService } from '../../core/services/account.service';
import { Appointment } from '../../shared/models/appointment';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-appointment',
  providers: [provideNativeDateAdapter()],
  imports: [
    ReactiveFormsModule,
    MatFormField,
    MatLabel,
    MatButton,
    MatInputModule,
    MatDatepickerModule,
    MatRadioModule,
    MatStepperModule,
    MatSelectModule,
    RouterLink,
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
  pets: Pet[] = [];
  
  ngOnInit(): void {
    this.servicesService.getServices().subscribe({
      next: response => this.services = response,
      error: error => console.log(error)
    })
    this.petService.getPetsByOwner().subscribe({
      next: pets => {
        this.pets = pets
      }
    })
  }

  appointmentForm = this.fb.group({
    appointmentDate: [''],
    petid: [''],
    ownerid: [''],
    serviceid: [''],
    notes: [''],
    clinicid: [''],
  });

  servicesForm = this.fb.group({
    serviceId: ['']
  });

  async onSubmit() {
    const currentUser = this.accountSerive.currentUser();

    if (!currentUser) {
      console.error('No logged-in user found,');
      return;
    }

    const ownerid = currentUser.id;
    const selectedPetId = this.appointmentForm.get('petid')?.value;
    const selectedService = Number(this.servicesForm.get('serviceId')?.value);
    const clinicId = 1;
    const date = this.appointmentForm.value.appointmentDate;
    const formattedDate = date ? new Date(date).toLocaleDateString('en-CA').split('T')[0] : '';
    const data = {
          serviceId: selectedService,
          ownerId: ownerid,
          petId: selectedPetId,
          appointmentDate : formattedDate,
          clinicid: clinicId
    };

    console.log(data);
    this.appointmentService.createAppointment(data).subscribe({
      next: (response : Appointment) => {
        this.appointmentService.appointmentComplete = true;
        this.router.navigate(['/appointment-success', response.id]);
      },
      error: err => console.error('Appointment error', err)
    })
  }

}
