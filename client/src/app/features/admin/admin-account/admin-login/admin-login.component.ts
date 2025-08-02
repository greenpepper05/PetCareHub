import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCard } from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { AccountService } from '../../../../core/services/account.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatInput } from '@angular/material/input';

@Component({
  selector: 'app-admin-login',
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
  templateUrl: './admin-login.component.html',
  styleUrl: './admin-login.component.scss'
})
export class AdminLoginComponent {
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
            this.router.navigate(['admin/dashboard']);
          }
        })
      },
      error: err => {
        console.error('Login failed: ', err);
      }
    })
  }
}
