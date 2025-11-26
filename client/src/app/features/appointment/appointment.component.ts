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
import { ClinicSchedule } from '../../shared/models/clinicSchedule';
import { Slot } from '../../shared/models/slot';

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
  clinicSchedules: ClinicSchedule[] = [];

  minDate = new Date();
  bookedSlots: string[] = [];

  isDateDisabled = (date: Date | null): boolean => {
    if (!date || this.clinicSchedules.length === 0) return true;

    const day = date.getDay();

    const schedule = this.clinicSchedules.find(x => x.dayOfWeek === day);

    return !!(schedule && schedule.isOpen);

  }

  getAvailableTimeSlot(): Slot[] {
    const selectedDate = this.appointmentForm.get('appointmentDate')?.value;

    if (!selectedDate || this.clinicSchedules.length === 0 ) return [];

    const day = selectedDate.getDay();
    const schedule = this.clinicSchedules.find(x => x.dayOfWeek === day);

    if (!schedule || !schedule.isOpen) return [];

    const opening = schedule.openingTime.substring(0,5);
    const closing = schedule.closingTime.substring(0,5);

    const slots: Slot[] = [];
    let [openHour] = opening.split(':').map(Number);
    let [closeHour] = closing.split(':').map(Number);

    for (let hour = openHour; hour < closeHour; hour++) {
      const time = `${hour.toString().padStart(2, '0')}:00`;
      slots.push({
        time,  
        isBooked: this.bookedSlots.includes(time),
        isPast: this.isPastSlot(selectedDate, time)
        // `${hour.toString().padStart(2,'0')}:00`);
      });
    }

    return slots;
  }

  // filterPastTimesIfToday(date: Date, slots: string[]): string[] {
  //   const now = new Date();

  //   const isToday = 
  //     date.getFullYear() === now.getFullYear() &&
  //     date.getMonth() === now.getMonth() &&
  //     date.getDate() === now.getDate();
    
  //   if (!isToday) return slots;

  //   const nowMinutes = now.getHours() * 60  + now.getMinutes();

  //   return slots.filter(time => {
  //     const [h, m] = time.split(':').map(Number);
  //     return (h * 60 + m) > nowMinutes;
  //   })
  // }
  
  ngOnInit(): void {

    var clinicId = this.activatedRoute.snapshot.paramMap.get("id");
    if (!clinicId) return;

    this.appointmentForm.get('appointmentDate')?.valueChanges.subscribe(date => {
      if (date) this.loadBokedSlots(date);
    })

    this.servicesService.getServiceByClinic(+clinicId).subscribe({
      next: response => this.services = response,
      error: error => console.log(error)
    })
    this.petService.getPetsByOwner().subscribe({
      next: pets => {
        this.pets = pets
      }
    })

    this.clinicService.getClinicSchedules(+clinicId).subscribe({
      next: schedule => {
        this.clinicSchedules = schedule;
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

  loadBokedSlots(date: Date) {
    const clinicId = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();

    this.appointmentService
      .getBookedSlots(clinicId, year, month, day)
      .subscribe(slots => {
        this.bookedSlots = slots;
      })
  }

  isPastSlot(date: Date, time: string): boolean {
    const now = new Date();
    const [h,m] = time.split(':').map(Number);
    
    const slotMinutes = h * 60 + m;
    const nowMinutes = now.getHours() * 60 + now.getMinutes();

    const isToday = 
      date.getFullYear() === now.getFullYear() &&
      date.getMonth() === now.getMonth() &&
      date.getDate() === now.getDate();

    return isToday && slotMinutes <= nowMinutes;
  }

  selectSlot(slot: any) {
    if (slot.isBooked || slot.isPast) return;
    this.appointmentForm.patchValue({ appointmentTime: slot.time });
  }

}
