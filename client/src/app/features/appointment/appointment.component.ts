import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormField, MatHint, MatLabel } from '@angular/material/form-field';
import { AppointmentService } from '../../core/services/appointment.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
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
import { ClinicService } from '../../core/services/clinic.service';
import { MatIcon } from '@angular/material/icon';
import { CurrencyPipe } from '@angular/common';

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
    MatIcon,
    CurrencyPipe
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
  private clinicService = inject(ClinicService);
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  services: any[] = [];
  pets: Pet[] = [];

  minDate = new Date();
  

  availableTimeSlots: string[] = [
    '09:00', '10:00', '11:00', '12:00', '13:00', 
    '14:00', '15:00', '16:00', '17:00'
  ];
  
  ngOnInit(): void {

    var id = this.activatedRoute.snapshot.paramMap.get("id");
    if (!id) return;

    this.servicesService.getServiceByClinic(+id).subscribe({
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
    appointmentDate: [null as Date | null, Validators.required],
    appointmentTime: ['', Validators.required],
    petid: ['', Validators.required],
    ownerid: ['', Validators.required],
    serviceid: ['', Validators.required],
    notes: ['', Validators.required],
    clinicid: ['', Validators.required],
  });

  getFilteredDate(): string[] {
    const selectedDate = this.appointmentForm.get('appointmentDate')?.value as Date | null;
    const now = new Date();

    if (!selectedDate) return this.availableTimeSlots;

    const isToday =
      selectedDate.getDate() === now.getDate() &&
      selectedDate.getMonth() === now.getMonth() &&
      selectedDate.getFullYear() === now.getFullYear();

    if (!isToday) return this.availableTimeSlots;

    const currentHour = now.getHours();
    const currentMinutes = now.getMinutes();

    return this.availableTimeSlots.filter((time) => {
      const [hours, minute] = time.split(':').map(Number);

      const slotMinutes = hours * 60 + minute;
      const nowMinutes = currentHour * 60 + currentMinutes;

      return slotMinutes > nowMinutes;
    })
  }

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

    return `${startDisplay}`;
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

    const id = this.activatedRoute.snapshot.paramMap.get("id");

    const clinicId = id;
    
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
