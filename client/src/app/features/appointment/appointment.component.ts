import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
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

  availableTimeSlots: string[] = [
    '09:00', '10:00', '11:00', '12:00', '13:00', 
    '14:00', '15:00', '16:00', '17:00'
  ];
  
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
    appointmentDate: [null, Validators.required],
    appointmentTime: ['', Validators.required],
    petid: ['', Validators.required],
    ownerid: ['', Validators.required],
    serviceid: ['', Validators.required],
    notes: ['', Validators.required],
    clinicid: ['', Validators.required],
  });

  formatTime(time: string): string {
    const [startHours, startMinutes] = time.split(':').map(Number);

    const startDate = new Date();
    startDate.setHours(startHours, startMinutes, 0,0);

    const endDate = new Date(startDate.getTime());
    endDate.setHours(startDate.getHours() + 1);

    const timeFormatOptions: Intl.DateTimeFormatOptions = {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    }
    
    const startDisplay = startDate.toLocaleTimeString('en-US', timeFormatOptions);
    const endDisplay = endDate.toLocaleTimeString('en-US', timeFormatOptions);

    return `${startDisplay} - ${endDisplay}`;
  }

  servicesForm = this.fb.group({
    serviceId: ['']
  });

  async onSubmit() {
    
    const currentUser = this.accountSerive.currentUser();

    if (!currentUser) {
      console.error('No logged-in user found,');
      return;
    }


    const datePart: Date = this.appointmentForm.value.appointmentDate!;
    const timePart: string = this.appointmentForm.value.appointmentTime!;

    const pad = (num: number) => num < 10 ? '0' + num : num.toString();

    const year = datePart.getFullYear();
    const month = pad(datePart.getMonth() + 1);
    const day = pad(datePart.getDate());
    
    
    const [hours, minutes] = timePart.split(':');
    
    const appointmentDateLocalString = `${year}-${month}-${day}T${hours}:${minutes}:00`;


    const ownerId = currentUser.id;

    const selectedPetId = this.appointmentForm.get('petid')?.value;

    const selectedService = Number(this.servicesForm.get('serviceId')?.value);


    const clinicId = 1; 
    
    const data = {
      serviceId: selectedService,
      ownerId: ownerId,
      petId: selectedPetId,
      appointmentDate : appointmentDateLocalString,
      clinicId: clinicId
    };

    console.log('Final Payload (Local Time String):', data);

    this.appointmentService.createAppointment(data).subscribe({
      next: (response : Appointment) => {
        this.appointmentService.appointmentComplete = true;
        this.router.navigate(['/appointment-success', response.id]);
      },
      error: err => console.error('Appointment error', err)
    })
  }

}
