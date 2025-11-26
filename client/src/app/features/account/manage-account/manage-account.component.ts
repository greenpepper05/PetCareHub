import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { PasswordInputComponent } from "../../../shared/components/password-input/password-input.component";
import { MatIcon } from '@angular/material/icon';
import { AccountService } from '../../../core/services/account.service';
import { User } from '../../../shared/models/user';
import { passwordValidator } from '../../../shared/validators/password.validators';
import { matchValidator } from '../../../shared/validators/matchValidator.Validators';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manage-account',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatButton,
    MatFormField,
    MatLabel,
    MatInput,
    MatError,
    MatIcon
  ],
  templateUrl: './manage-account.component.html'
})
export class ManageAccountComponent implements OnInit {
  private accountService = inject(AccountService);
  private snackbarService = inject(SnackbarService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  user!: User;

  isEditMode = false;

  accountForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    contact: ['', Validators.required],
    oldPassword: ['', Validators.required],
    newPassword: ['', [Validators.required, passwordValidator()]],
    confirmPassword: ['', Validators.required]
  }, { validators: matchValidator('newPassword', 'confirmPassword')});

  ngOnInit(): void {
    this.loadUser();
    this.populateForm();
  }

  populateForm() {
    this.accountForm = this.fb.group({
      firstName: [this.user.firstName, Validators.required],
      lastName: [this.user.lastName, Validators.required],
      email: [this.user.email, [Validators.required, Validators.email]],
      contact: [this.user.contact, Validators.required],
      oldPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, passwordValidator()]],
      confirmPassword: ['', Validators.required]
    })
  }

  loadUser() {
    const user = this.accountService.currentUser();
    
    if (!user) return;

    return this.user = user;
  }

  toggleEdit() {
    this.isEditMode = !this.isEditMode;
  }

  saveChanges() {
    if (this.accountForm.invalid) return;

    const userId = this.accountService.currentUser()!.id;

    if (!userId) return;

    const updatedData = this.accountForm.value as any;

    console.log("Updating account:", updatedData);

    // if (!updatedData) return; 

    this.accountService.updateUser(userId, updatedData).subscribe({
      next: () => {
        this.snackbarService.success("User successfully updated");
        this.accountService.logout();
        this.router.navigateByUrl("/account/login");
        this.snackbarService.success("Please re-login you account");
      }
    })

    this.isEditMode = false;
  }
}
