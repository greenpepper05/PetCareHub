import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { PetServiceHistoryService } from '../../../../core/services/pet-service-history.service';
import { PetServiceHistory } from '../../../../shared/models/petServiceHistory';
import { CurrencyPipe, DatePipe, NgClass } from '@angular/common';
import { PetService } from '../../../../core/services/pet.service';
import { AccountService } from '../../../../core/services/account.service';
import { Pet } from '../../../../shared/models/pet';
import { User } from '../../../../shared/models/user';
import { MatButton } from '@angular/material/button';
import { ServiceRecordService } from '../../../../core/services/service-record.service';
import { ServiceRecord } from '../../../../shared/models/serviceRecord';
import { Procedures } from '../../../../shared/models/procedures';
import { ProceduresService } from '../../../../core/services/procedures.service';
import { ServiceRecordStep } from '../../../../shared/models/serviceRecordStep';
import { ServiceRecordStepService } from '../../../../core/services/service-record-step.service';
import { MatIcon } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfimationDialogComponent } from '../../../../shared/components/confimation-dialog/confimation-dialog.component';
import { finalize, tap } from 'rxjs';
import { SelectStaffComponent } from '../../../../shared/components/select-staff/select-staff.component';
import { SnackbarService } from '../../../../core/services/snackbar.service';

@Component({
  selector: 'app-pet-service-detail',
  imports: [
    DatePipe,
    CurrencyPipe,
    MatButton,
    RouterLink,
    MatIcon,
    NgClass,
    MatDialogModule
  ],
  templateUrl: './pet-service-detail.component.html',
  styleUrl: './pet-service-detail.component.scss'
})
export class PetServiceDetailComponent implements OnInit{
  private activatedRoute = inject(ActivatedRoute);
  private serviceRecord = inject(ServiceRecordService);
  private serviceRecordStep = inject(ServiceRecordStepService);
  private procedureService = inject(ProceduresService);
  private snackbarService = inject(SnackbarService);
  private petService = inject(PetService);
  private user = inject(AccountService);
  private dialog = inject(MatDialog);
  private router = inject(Router);
  services?: ServiceRecord;
  records?: ServiceRecordStep[] = [];
  procedures?: (Procedures & { isSkipped?: boolean })[] = [];
  pet?: Pet;
  owner?: User;


  ngOnInit(): void {
    this.loadPetHistory();

  }

  loadPetHistory() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;

    this.serviceRecord.getServiceRecordByPetId(+id).subscribe({
      next: (history) => {
        this.services = history;
        this.loadPet(history.petId);
        this.loadProcedure(history.id);
      } 
    })
  }

  loadPet(petId: number) {
    this.petService.getPetById(petId).subscribe({
      next: (pets) => {
        this.pet = pets
        const userId = pets.ownerId
        this.loadOwner(userId);
      }
    })
  }

  loadOwner(userId: string) {
    this.user.getUserById(userId).subscribe({
      next: (userInfo) => {
        if (userInfo.id === userId) {
          this.owner = userInfo;
        }
      }
    })
  }
  
  loadProcedure(serviceId: number) {

    if (!serviceId) return;

    this.procedureService.getProcedures(+serviceId).subscribe({
      next: (record) => {
        this.procedures = record
      }
    })

  }

  deleteRecord(recordId?: number): void {
    if (!recordId) return;

    const dialogRef = this.dialog.open(ConfimationDialogComponent, {
        width: '350px',
        data: {
            title: 'Confirm Deletion',
            message: 'Are you sure you want to permanently delete this service record? This action cannot be undone.'
        }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
          this.serviceRecord.deleteServiceRecord(+recordId).pipe(
              tap(() => console.log("Record Deleted successfully (TAP triggered)")),
              finalize(() => {
                  this.router.navigateByUrl('/admin/service-record'); 
              })
          ).subscribe({
              next: () => {
              },
              error: (err) => {
                  if (err.status === 204) {
                      console.log("Delete API returned 204 No Content - Success assumed.");
                  } else {
                      console.error('Failed to delete record:', err);
                      // Handle true errors (e.g., 500 server error) here
                  }
              }
          });
      } else {
          console.log("Deletion cancelled by user.");
      }
  });
  }

  onSkipChange(procedure: Procedures & { isSkipped?: boolean}, event: Event): void {
    if (!this.services?.id || !procedure.id) return;

    const isChecked = (event.target as HTMLInputElement).checked;

    const newIsSkippedStatus = !isChecked;

    this.serviceRecordStep.skipServiceRecordStep(this.services.id, procedure.id, newIsSkippedStatus).subscribe({
      next: () => {
        if (this.procedures) {
          this.procedures = this.procedures.map(p => p.id === procedure.id ? { ...p, isSkipped: newIsSkippedStatus} : p)
        };
      },
      error: (err) => {
        console.error('Failed to update skip status:', err);
        (event.target as HTMLInputElement).checked = isChecked;
      }
    })
  }

  openStaffSelection() {
    const dialogRef = this.dialog.open(SelectStaffComponent, {
      width: '450px',
    });

    dialogRef.afterClosed().subscribe(selectedStaffId => {
      if (!selectedStaffId) return;
      this.serviceRecord.startProcedure(this.services!.id).subscribe({
        next: () => {
          this.serviceRecord.assignStaff(this.services!.id, selectedStaffId).subscribe({
            next: () => {
              this.snackbarService.success("Staff assigned!");
              this.router.navigate(
                ['admin', 'service-record', this.services!.id, 'procedure'],
                { queryParams: { staffId: selectedStaffId }}
              );
            }
          })
        }
      })
      
    })
  }

  viewProcedure() {
    this.router.navigate(
      ['admin', 'service-record', this.services!.id, 'procedure']
    )
  }

}
