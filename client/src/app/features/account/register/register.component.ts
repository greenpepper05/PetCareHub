import { Component, inject } from '@angular/core';
import { EmailValidator, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../../../core/services/account.service';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { passwordValidator } from '../../../shared/validators/password.validators';
import { matchValidator } from '../../../shared/validators/matchValidator.Validators';
import { MatIconModule } from '@angular/material/icon';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { OtpService } from '../../../core/services/otp.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [
    MatButton,
    MatFormField,
    MatLabel,
    ReactiveFormsModule,
    MatInput,
    RouterLink,
    MatError,
    MatIconModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})

export class RegisterComponent {
  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router);
  private snackbarService = inject(SnackbarService);
  public otpService = inject(OtpService);

  public isOtpSent = false;

  registerForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    contact: ['', Validators.required],
    password: ['', [Validators.required, passwordValidator()]],
    confirmPassword: ['', Validators.required],
    clinicId: [null],
    otp: ['', Validators.pattern('^[0-9]{6}$')]
  }, { validators: matchValidator('password', 'confirmPassword') });

  // async onSubmit() {
    
  //   const payload = {
  //     firstName: this.registerForm.value.firstName,
  //     lastName: this.registerForm.value.lastName,
  //     email: this.registerForm.value.email,
  //     contact: this.registerForm.value.contact,
  //     password: this.registerForm.value.password,
  //     clinicId: this.registerForm.value.clinicId || null
  //   }

  //   this.accountService.register(payload).subscribe({
  //     next: () => {
  //       this.router.navigateByUrl('/account/login');
  //       this.snackbarService.success("Registration successful");
  //     },
  //     error: (err) => {
  //       const errorMessage = err.error?.message || 'Registration failed, Please check your credentials.';
  //       this.snackbarService.error(errorMessage);
  //     }
  //   })
  // }

  async sendOtp() {
    const emailControl = this.registerForm.get('email');

    if (!emailControl?.value || emailControl.invalid) {
      emailControl?.markAsTouched();
      this.snackbarService.error('Please enter a valid email before sending OTP.');
      return;
    }

    const emailPayload = {
      email: this.registerForm.value.email,
      clinicId: this.registerForm.value.clinicId
    };

    this.accountService.sendOtp(emailPayload).subscribe({
      next: () => {
        this.snackbarService.success(`OTP sent to ${this.registerForm.value.email}`);
        this.isOtpSent = true;

        const otpControl = this.registerForm.get('otp');
        otpControl?.addValidators([Validators.required, Validators.minLength(6), Validators.maxLength(6)]);
        otpControl?.updateValueAndValidity();
      },
      error: (err) => {
        const errorMessage = err.error?.message || 'Failed to send OTP. Please try again.';
        this.snackbarService.error(errorMessage);
      }
    })
  }

  async verifyOtpAndRegistration() {
    if (this.registerForm.invalid) {
      this.registerForm.markAsTouched();
      this.snackbarService.error("Please enter a valid 6-digit OTP.");
      return;
    }

    const fullRegistrationData = this.registerForm.value as any;
    const otp = this.registerForm.value.otp!;

    this.otpService.verifyOtpAndRegister(otp, fullRegistrationData).subscribe({
      next: () => {
        this.router.navigateByUrl('/account/login');
        this.snackbarService.success("Registration successful! You can now log in.");
      },
      error: (err) => {
        const errorMessage = err.error.message || "OTP verification failed or registration error.";
        this.snackbarService.error(errorMessage);
      }
    })
  }
}
