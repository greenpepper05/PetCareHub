import { Component, inject } from '@angular/core';
import { ProcedureService } from '../../../core/services/procedure.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-procedure-modal',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './add-procedure-modal.component.html',
  styleUrl: './add-procedure-modal.component.scss'
})
export class AddProcedureModalComponent {
  private dialogRef = inject(MatDialogRef);
  private fb = inject(FormBuilder);
  private procedureService = inject(ProcedureService);
  private activatedRoute = inject(ActivatedRoute);
  private data: { serviceId: number } = inject(MAT_DIALOG_DATA);

  procedureForm: FormGroup;

  constructor() {
    this.procedureForm = this.fb.group({
      serviceId: [this.data.serviceId],
      name: ['', Validators.required],
      description: [''],
    });
  }

  onSubmit(): void {
    if (this.procedureForm.valid) {
      const newProcedure = this.procedureForm.value;
      const serviceId = this.data.serviceId;
      this.procedureService.createProcedure(newProcedure, serviceId).subscribe({
        next: (proc) => {
          this.dialogRef.close(proc);
        },
        error: (err) => console.error('Failed to create procedure', err)
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
