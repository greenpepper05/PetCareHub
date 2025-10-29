import { Component, inject, signal } from '@angular/core';
import { ServicesService } from '../../../../core/services/services.service';
import { ProcedureService } from '../../../../core/services/procedure.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Services } from '../../../../shared/models/services';
import { Procedures } from '../../../../shared/models/procedures';
import { CdkDragDrop, moveItemInArray, CdkDrag, CdkDropList } from '@angular/cdk/drag-drop';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AddProcedureModalComponent } from '../../../../shared/components/add-procedure-modal/add-procedure-modal.component';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-service-detail',
  imports: [
    CdkDrag,
    CdkDropList,
    RouterLink,
    MatDialogModule,
    MatIcon
  ],
  templateUrl: './service-detail.component.html',
  styleUrl: './service-detail.component.scss'
})
export class ServiceDetailComponent {
  private servicesService = inject(ServicesService);
  private procedureService = inject(ProcedureService);
  private activatedRoute = inject(ActivatedRoute);
  private dialog = inject(MatDialog);

  service = signal<Services | undefined>(undefined);
  procedures = signal<Procedures[]>([]);
  editingMode = signal(false);
  editingProcedureId = signal<number | null>(null);

  ngOnInit(): void {
    const serviceId = this.activatedRoute.snapshot.paramMap.get('id');
    if (serviceId) {
      this.loadServiceDetails(+serviceId);
    }
  }

  loadServiceDetails(serviceId: number): void {
    this.servicesService.getService(serviceId).subscribe({
      next: (svc) => {
        this.service.set(svc);
        this.loadProcedures(serviceId);
      },
      error: (err) => console.error('Failed to load service details:', err)
    });
  }

  loadProcedures(serviceId: number): void {
    this.procedureService.getProcedures(serviceId).subscribe({
      next: (procs) => this.procedures.set(procs.sort((a, b) => a.order - b.order)),
      error: (err) => console.error('Failed to load procedures:', err)
    });
  }

  toggleEditing(): void {
    this.editingMode.update(value => !value);
  }

  onDrop(event: CdkDragDrop<string[]>) {
    const proceduresCopy = [...this.procedures()];
    moveItemInArray(proceduresCopy, event.previousIndex, event.currentIndex);

    // Update the 'order' property of each procedure in the reordered array
    proceduresCopy.forEach((proc, index) => {
      proc.order = index;
    });

    this.procedures.set(proceduresCopy);

    // Connect to the API to save the new order
    if (this.service()) {
      this.procedureService.reorderProcedures(this.service()!.id, this.procedures()).subscribe({
        next: () => console.log('Successfully saved new procedure order.'),
        error: (err) => console.error('Failed to save new procedure order:', err)
      });
    }
  }

  openAddProcedureModal(): void {
    const serviceId = this.service()?.id;
    if (!serviceId) return;

    const dialogRef = this.dialog.open(AddProcedureModalComponent, {
      width: '450px',
      data: { serviceId }
    });

    dialogRef.afterClosed().subscribe((result: Procedures) => {
      if (result) {
        // Refresh the list to include the newly added procedure
        this.loadProcedures(serviceId);
      }
    });
  }

  deleteProcedure(id: number): void {
    this.procedureService.deleteProcedure(id).subscribe({
      next: () => {
        this.procedures.update(procs => procs.filter(proc => proc.id !== id));
      },
      error: (err) => console.error('Failed to delete procedure:', err)
    });
  }

  startEditing(id: number): void {
    this.editingProcedureId.set(id);
  }

  saveEdit(): void {
    console.log('saveEdit() function triggered');
    const procedureToUpdate = this.procedures().find(p => p.id === this.editingProcedureId());
    const serviceId = this.service()?.id;
    console.log('Procedure to update:', procedureToUpdate);
    console.log('Service ID:', serviceId);

    if (procedureToUpdate && serviceId) {
      this.procedureService.updateProcedure(procedureToUpdate).subscribe({
        next: () => {
          console.log('Successfully updated procedure.');
          this.editingProcedureId.set(null); // Exit editing mode
          this.loadProcedures(serviceId); // Reload procedures to reset UI state
        },
        error: (err) => console.error('Failed to update procedure:', err)
      });
    }
  }

  cancelEdit(): void {
    this.editingProcedureId.set(null);
  }

  updateProcedure(id: number, key: 'name' | 'description', event: Event): void {
    const value = (event.target as HTMLInputElement | HTMLTextAreaElement).value;
    this.procedures.update(procs => 
      procs.map(proc => 
        proc.id === id ? { ...proc, [key]: value } : proc
      )
    );
  }
}
