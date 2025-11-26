import { Component, Inject, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../core/services/account.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { OtpService } from '../../../core/services/otp.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { passwordValidator } from '../../validators/password.validators';
import { matchValidator } from '../../validators/matchValidator.Validators';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';


@Component({
  selector: 'app-admin-change-password',
  imports: [
    MatFormField,
    MatInput,
    MatLabel,
    MatError,
    MatIcon,
    MatButton,
    ReactiveFormsModule
  ],
  templateUrl: './admin-change-password.component.html',
  styleUrl: './admin-change-password.component.scss'
})
export class AdminChangePasswordComponent {
  private accountService = inject(AccountService);
  private snackbarService = inject(SnackbarService);
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<AdminChangePasswordComponent>)


  changePasswordForm = this.fb.group({
    oldPassword: ['', Validators.required],
    newPassword: ['', [Validators.required, passwordValidator()]],
    confirmPassword: ['', Validators.required],
  }, { validators: matchValidator('newPassword', 'confirmPassword')});


  submit() {
    const id = this.accountService.currentUser()?.id;
    if (!id) return;

    if (this.changePasswordForm.invalid) return;

    const data = this.changePasswordForm.value as any;

    this.accountService.changePassword(id, data).subscribe({
      next: () => {
        this.snackbarService.success("Password change successfully")
      }
    })
  }
}
