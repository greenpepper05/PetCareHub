import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, model, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatCalendar, MatDatepicker, MatDatepickerModule } from "@angular/material/datepicker";
import { AppointmentService } from '../../../core/services/appointment.service';
import { Appointment } from '../../../shared/models/appointment';
import { RouterLink } from '@angular/router';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../../shared/models/pagination';
import { AppointmentParams } from '../../../shared/models/appointmentParams';
import { MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { MatDialog } from '@angular/material/dialog';
import { FilterDialogComponent } from './filter-dialog/filter-dialog.component';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-appointments',
  providers: [provideNativeDateAdapter()],
  imports: [
    CommonModule,
    MatDatepickerModule,
    MatCardModule,
    DatePipe,
    RouterLink,
    MatPaginator,
],
  templateUrl: './appointments.component.html',
  styleUrl: './appointments.component.scss'
})
export class AppointmentsComponent implements OnInit{
  selected = model<Date | null>(null);
  private appointmentService = inject(AppointmentService);
  private dialogService = inject(MatDialog);
  appointments?: Pagination<Appointment>;
  sortOptions = [
    {name: 'Alpabetical', value: 'name'},
    {name: 'Date Latest to Oldest', value: 'latestToOldest'},
    {name: 'Date Oldest to Latest', value: 'oldestToLates'},
  ]
  appointmentParams = new AppointmentParams();
  pageSizeOptions = [10,20,30];


  ngOnInit(): void {
    this.getAppointments();
  }

  getAppointments() {
    this.appointmentService.getAppointmentByClinic(this.appointmentParams).subscribe({
      next: (response) => {
        this.appointments = response;
      }
    })
  }

  handlePageEvent(event: PageEvent) {
    this.appointmentParams.pageNumber = event.pageIndex + 1;
    this.appointmentParams.pageSize = event.pageSize;
    this.getAppointments();
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.appointmentParams.sort = selectedOption.value;
      this.appointmentParams.pageNumber = 1;
      this.getAppointments();
    }
  }

  // openFilterDialog() {
  //   const dialogRef = this.dialogService.open(FilterDialogComponent , {
  //     maxWidth: '500px',
  //     data: {
  //       sele
  //     }
  //   })
  // }

}
