import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../../../core/services/account.service';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { passwordValidator } from '../../../shared/validators/password.validators';

@Component({
  selector: 'app-register',
  imports: [
    MatButton,
    MatFormField,
    MatLabel,
    ReactiveFormsModule,
    MatInput,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})

export class RegisterComponent {
  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router);

  registerForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', Validators.required],
    contact: ['', Validators.required],
    password: ['', Validators.required, passwordValidator()],
    clinicId: [null]
  });

  onSubmit() {
    
    const payload = {
      firstName: this.registerForm.value.firstName,
      lastName: this.registerForm.value.lastName,
      email: this.registerForm.value.email,
      contact: this.registerForm.value.contact,
      password: this.registerForm.value.password,
      clinicId: this.registerForm.value.clinicId || null
    }

    this.accountService.register(payload).subscribe({
      next: () => {
        this.router.navigateByUrl('/account/login');
      }
    })
  }
}
