import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-superadmin-login',
  imports: [
    MatCard,
    MatFormField,
    MatLabel,
    MatButton,
    MatInput,
    ReactiveFormsModule,
    CommonModule,
    MatError
  ],
  templateUrl: './superadmin-login.component.html',
  styleUrl: './superadmin-login.component.scss'
})
export class SuperadminLoginComponent {
  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router);

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  onSubmit() {
    if (this.loginForm.invalid) return;

    const { email, password } = this.loginForm.value;

    const loginRequest = {
      email: email ?? '',
      password: password ?? ''
    }

    this.accountService.adminLogin(loginRequest).subscribe({
      next: () => {
        this.accountService.getUserInfo().subscribe({
          next: () => {
            this.router.navigate(['superadmin/dashboard']);
          }
        })
      },
      error: err => {
        console.error('Login failed: ', err);
      }
    })
  }
}
