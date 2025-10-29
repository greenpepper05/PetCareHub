import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ClinicService } from '../../../core/services/clinic.service';
import { AccountService } from '../../../core/services/account.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import { finalize } from 'rxjs';
import { SnackbarService } from '../../../core/services/snackbar.service';



@Component({
  selector: 'app-add-new-clinic',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIcon
],
  templateUrl: './add-new-clinic.component.html',
  styleUrl: './add-new-clinic.component.scss'
})
export class AddNewClinicComponent {
  private dialogRef = inject(MatDialogRef);
  private fb = inject(FormBuilder);
  private clinicService = inject(ClinicService);
  private accountService = inject(AccountService);
  private snackbar = inject(SnackbarService);

  selectedFile: File | null = null;
  imageUrl: string | ArrayBuffer | null = null;
  isUploading = false;

  form = this.fb.group({
    ownerId: [''],
    clinicName: [''],
    address: [''],
    phoneNumber: [''],
    email: [''],
    pictureUrl: ['']
  });

  onSubmit() {
    if (this.form.valid) {

      const currentUser = this.accountService.currentUser();

      if (!currentUser) return;

      const payload = {
        ownerId: this.form.value.ownerId,
        clinicName: this.form.value.clinicName,
        address: this.form.value.address,
        phoneNumber: this.form.value.phoneNumber,
        email: this.form.value.email,
        pictureUrl: this.form.value.pictureUrl,
  
      }

      this.clinicService.registerClinic(payload).subscribe({
        next: (response) => {
          this.snackbar.success("Clinic registered successfully!");
          this.dialogRef.close(response);
        },
        error: err => {
          const errorMessage = Array.isArray(err) ? err.join(', ') : 'Failed to register clinic.';
          this.snackbar.error(errorMessage);
        }
      });
    }
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];

    if (file) {
      this.selectedFile = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.imageUrl = reader.result;
      };
      reader.readAsDataURL(file);
      this.uploadImage();
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  uploadImage() : void {
    if (!this.selectedFile) {
      console.log("No Image");
      return;
    }
    
    this.isUploading = true;
    const formData = new FormData();

    formData.append('File', this.selectedFile, this.selectedFile.name);

    this.clinicService.uploadImage(formData)
      .pipe(
        finalize(() => this.isUploading = false)
      )
      .subscribe({
        next: (response) => {
          const pictureUrl = response.url;

          this.form.patchValue({
            pictureUrl: pictureUrl
          })
        },
        error: (err) => {
          console.error('Image upload failed: ', err);

          this.selectedFile = null;
          this.imageUrl = null;
          this.form.get('pictureUrl')?.setValue('');
        }
      })
  }
}
