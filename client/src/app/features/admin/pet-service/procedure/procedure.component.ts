import { CommonModule, DatePipe } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ServiceRecordService } from '../../../../core/services/service-record.service';
import { ServiceRecordStepService } from '../../../../core/services/service-record-step.service';
import { PetService } from '../../../../core/services/pet.service';
import { AccountService } from '../../../../core/services/account.service';
import { ServiceRecord } from '../../../../shared/models/serviceRecord';
import { ServiceRecordStep } from '../../../../shared/models/serviceRecordStep';
import { Pet } from '../../../../shared/models/pet';
import { User } from '../../../../shared/models/user';
import { ProcedureService } from '../../../../core/services/procedure.service';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-procedure',
  imports: [
    DatePipe,
    CommonModule,
    MatButton,
    RouterLink,
    MatIcon
],
  templateUrl: './procedure.component.html',
  styleUrl: './procedure.component.scss'
})
export class ProcedureComponent implements OnInit{
  private activatedRoute = inject(ActivatedRoute);
  private serviceRecord = inject(ServiceRecordService);
  private serviceRecordStep = inject(ServiceRecordStepService);
  private petService = inject(PetService);
  private procedure = inject(ProcedureService);
  private user = inject(AccountService);
  services = signal<ServiceRecord | undefined>(undefined);
  records = signal<(ServiceRecordStep & { isCompleted?: boolean})[]>([]);
  pet = signal<Pet | undefined>(undefined);
  owner = signal<User | undefined>(undefined);
  currentRecordIndex = signal<number>(0);

  filteredRecords = computed(() => this.records().filter(p => !p.isSkipped));
  currentRecord = computed(() => this.filteredRecords()[this.currentRecordIndex()]);
  isFirstRecord = computed(() => this.currentRecordIndex() === 0);
  isLastRecord = computed(() => this.currentRecordIndex() === this.filteredRecords().length - 1);

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.loadServiceRecord(+id);
    }
  }

  loadServiceRecord(id: number) {
    this.serviceRecord.getServiceRecordByPetId(id).subscribe({
      next: (history) => {
        this.services.set(history);
        this.loadPet(history.petId);
        this.loadRecords(history.id);
      },
      error: (err) => console.error('Failed to load service record', err)
    })
  }

  loadPet(petId: number) {
    this.petService.getPetById(petId).subscribe({
      next: (pet) => {
        this.pet.set(pet);
        this.loadOwner(pet.ownerId);
      },
      error: (err) => console.error('Failed to load pet', err)
    })
  }

  loadOwner(userId: string) {
    this.user.getUserById(userId).subscribe({
      next: (owner) => {
        this.owner.set(owner);
      },
      error: (err) => console.error('Failed to load owner', err)
    })
  }

  loadRecords(recordId: number) {
    this.serviceRecordStep.getServiceRecordStep(recordId).subscribe({
      next: (records) => {
        const recordWithStatus = records.map(p => ({ ...p, isCompleted: false}));
        this.records.set(recordWithStatus);
      },
      error: (err) => console.error('Failed to load records', err)
    });
  }

  nextRecord() {
    const currentRecord = this.currentRecord();
    const serviceRecordId = this.services()?.id;

    if (!this.currentRecord || !serviceRecordId) {
      console.error('Missing current record or service record ID');
      return;
    }

    this.procedure.stepComplete(serviceRecordId, currentRecord.id).subscribe({
      next: () => {
        this.records.update(recs => {
          return recs.map(r => r.id == currentRecord.id ? { ...r, isCompleted: true} : r);
        });

        if (!this.isLastRecord()) {
          this.currentRecordIndex.update(i => i + 1);
        } else {
          this.procedure.procedureComplete(serviceRecordId).subscribe({
            next: () => {
             this.loadServiceRecord(serviceRecordId); 
            }
          });
          alert('All records completed!');
        }
      }
    });

  }

  previousRecord() {
    if (!this.isFirstRecord()) {
      this.currentRecordIndex.update(i => i -1);
    }
  }


}
