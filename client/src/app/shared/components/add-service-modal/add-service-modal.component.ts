import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ServicesService } from '../../../core/services/services.service';

@Component({
  selector: 'app-add-service-modal',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './add-service-modal.component.html',
  styleUrl: './add-service-modal.component.scss'
})
export class AddServiceModalComponent {
  private dialogRef = inject(MatDialogRef);
  private fb = inject(FormBuilder);
  private servicesService = inject(ServicesService);

  serviceForm: FormGroup;

  constructor() {
    this.serviceForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      price: [0, Validators.min(0)]
    });
  }

  onSubmit(): void {
    if (this.serviceForm.valid) {
      const newService = this.serviceForm.value;
      this.servicesService.createService(newService).subscribe({
        next: (service) => {
          this.dialogRef.close(service);
        },
        error: (err) => console.error('Failed to create service', err)
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
