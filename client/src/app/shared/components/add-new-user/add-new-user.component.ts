import { Component, inject } from '@angular/core';
import { AccountService } from '../../../core/services/account.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { last } from 'rxjs';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormField, MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-add-new-user',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './add-new-user.component.html',
  styleUrl: './add-new-user.component.scss'
})
export class AddNewUserComponent {
  private dialogRef = inject(MatDialogRef);
  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);
  private snacbar = inject(SnackbarService);
  
  registerForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', Validators.required],
    contact: ['', Validators.required],
    clinicId: [null]
  });

  onSubmit() {
    if (this.registerForm.valid) {

      const currentUser = this.accountService.currentUser();

      if (!currentUser) return;

      const payload = {
        firstName: this.registerForm.value.firstName,
        lastName: this.registerForm.value.lastName,
        email: this.registerForm.value.email,
        contact: this.registerForm.value.contact,
      }

      this.accountService.registerAdminUser(payload).subscribe({
        next: (response) => {
          this.snacbar.success("Admin user registered successfully!");
          this.dialogRef.close(response);
        },
        error: err => {
          const errorMessage = Array.isArray(err) ? err.join(', ') : 'Failed to register user';
          this.snacbar.error(errorMessage);
        }
      })
    }
  }
  
  onCancel(): void {
    this.dialogRef.close();
  }
}
