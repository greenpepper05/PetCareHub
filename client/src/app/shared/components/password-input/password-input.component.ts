import { Component, Input } from '@angular/core';
import { AbstractControl, Form, FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-password-input',
  imports: [
    MatError,
    MatFormField,
    ReactiveFormsModule,
    MatLabel,
    
  ],
  templateUrl: './password-input.component.html',
  styleUrl: './password-input.component.scss'
})
export class PasswordInputComponent {
  @Input({ required: true}) control!: AbstractControl;
  @Input() label: string = "Password";

  get c() : FormControl {
    return this.control as FormControl;
  }
}
