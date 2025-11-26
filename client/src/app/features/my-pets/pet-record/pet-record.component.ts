import { Component, inject, OnInit } from '@angular/core';
import { ServiceRecordService } from '../../../core/services/service-record.service';
import { ServiceRecord } from '../../../shared/models/serviceRecord';
import { AccountService } from '../../../core/services/account.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { ActivatedRoute } from '@angular/router';
import { DatePipe, NgClass } from '@angular/common';
    import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-pet-record',
  imports: [
    NgClass,
    DatePipe,
    MatProgressSpinnerModule 
  ],
  templateUrl: './pet-record.component.html',
  styleUrl: './pet-record.component.scss'
})
export class PetRecordComponent implements OnInit {
  private accountService = inject(AccountService);
  private serviceRecords = inject(ServiceRecordService);
  private activatedRoute = inject(ActivatedRoute);
  private snackbarService = inject(SnackbarService);
  serviceRecord?: ServiceRecord;

  userId = this.accountService.currentUser()?.id;

  ngOnInit(): void {
    this.loadServiceRecord();
  }

  loadServiceRecord() {
    const petId = this.activatedRoute.snapshot.paramMap.get("id");
    const serviceId = this.activatedRoute.snapshot.paramMap.get("serviceId");

    if (!petId) return;
    if (!serviceId) return;

    this.serviceRecords.getRecordByPetIdAndServiceId(+petId, +serviceId).subscribe({
      next: (response) => {
        this.serviceRecord = response
       
      }
    })
  }

}
