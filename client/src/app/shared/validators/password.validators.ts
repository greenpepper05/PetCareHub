import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";


export function passwordValidator(): ValidatorFn {
    return (control: AbstractControl) : ValidationErrors | null => {
        const value = control.value;
        if (!value) {
            return null;
        }

        const hasMinLength = value.length >= 8;
        const hasCapital = /[A-Z]+/.test(value);
        const hasNumber = /[0-9]+/.test(value);
        const hasSpecial = /[^a-zA-Z0-9]+/.test(value);

        const validationErrors: ValidationErrors = {}
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

        return Object.keys(validationErrors).length > 0 ? validationErrors : null;
    }
}