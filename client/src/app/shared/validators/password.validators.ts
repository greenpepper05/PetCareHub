import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";


export function passwordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    
    // Allow empty value if Validators.required is not applied, or if the control hasn't been touched yet.
    if (!value) {
      return null;
    }

    const valueString = String(value); // Ensure we are working with a string
    
    const hasMinLength = valueString.length >= 8;
    const hasCapital = /[A-Z]+/.test(valueString);
    const hasNumber = /[0-9]+/.test(valueString);
    const hasSpecial = /[^a-zA-Z0-9]+/.test(valueString);

    // Return specific errors for each failed requirement
    const validationErrors: ValidationErrors = {};
    if (!hasMinLength) {
      validationErrors['minlength'] = true;
    }
    if (!hasCapital) {
      validationErrors['capitalRequired'] = true;
    }
    if (!hasNumber) {
      validationErrors['numberRequired'] = true;
    }
    if (!hasSpecial) {
      validationErrors['specialRequired'] = true;
    }

    // Return the errors object if there are errors, otherwise return null (valid)
    return Object.keys(validationErrors).length > 0 ? validationErrors : null;
  };
}