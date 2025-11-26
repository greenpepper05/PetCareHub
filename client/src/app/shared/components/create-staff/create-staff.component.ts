import { Component, inject, OnInit } from '@angular/core';
import { StaffService } from '../../../core/services/staff.service';
import { AccountService } from '../../../core/services/account.service';
import { CreateStaff } from '../../models/createStaff';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';
import { catchError, concatMap, finalize, map, Observable, of } from 'rxjs';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-create-staff',
  imports: [
    MatIcon,
    MatFormField,
    MatLabel,
    MatError,
    ReactiveFormsModule,
    MatInputModule
  ],
  templateUrl: './create-staff.component.html',
  styleUrl: './create-staff.component.scss'
})
export class CreateStaffComponent{
  private staffService = inject(StaffService);
  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private snackbarService = inject(SnackbarService);
  private dialogRef = inject(MatDialogRef<CreateStaffComponent>);

  selectedFile: File | null = null;
  imageUrl: string | ArrayBuffer | null = null;
  isUploading = false;

  clinicId = this.accountService.currentUser()?.clinicId;

  staffForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', Validators.required],
    staffRole: ['', Validators.required],
    pictureUrl: [''],
    clinicId: this.clinicId
  })

  createStaff() {
    if (this.staffForm.invalid) {
      this.staffForm.markAllAsTouched();
      return;
    }

    this.isUploading = true;

    let upload: Observable<{url: string}>;

    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('File', this.selectedFile, this.selectedFile.name);

      upload = this.staffService.uploadImage(formData).pipe(
        map(response => ({ url: response.url})),
        catchError(err => {
            this.snackbarService.error(`Image upload failed. Proceeding without picture: ${err}`)
            return of({ url: ''});
        })
      );
    } else {
      upload = of({ url: ''})
    }
    upload.pipe(
      concatMap(uploadResult => {
        const finalPayload = {
          ...this.staffForm.value,
          pictureUrl: uploadResult.url
        };
        return this.staffService.createStaff(finalPayload);
      }),
      finalize(() => this.isUploading = false)
    ).subscribe({
      next: () => {
        this.snackbarService.success("Staff create successfully")
      }, 
      error: (err) => {
        this.snackbarService.error(`Staff registration failed ${err}`);
      }
    })

  }

  close() {
    this.dialogRef.close();
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
    }
  }

  uploadImage() : void {
    if (!this.selectedFile) return;

    this.isUploading = true;
    const formData = new FormData();

    formData.append('File', this.selectedFile, this.selectedFile.name);

    this.staffService.uploadImage(formData).pipe(
      finalize(() => this.isUploading = false)
    )
    .subscribe({
      next: (response) => {
        const pictureUrl = response.url;

        this.staffForm.patchValue({
          pictureUrl: pictureUrl
        })
      },
      error: (err) => {
        this.selectedFile = null;
        this.imageUrl = null;
        this.staffForm.get('pictureUrl')?.setValue('');
      }
    })
  }

}
