import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function matchValidator(controlName: string, checkControlName: string): ValidatorFn { 
    return (group: AbstractControl): ValidationErrors | null => {
        const control = group.get(controlName);
        const checkControl = group.get(checkControlName);

        if (!control || !checkControl || checkControl.errors && checkControl.errors['required']) return null;

        if (control.value !== checkControl.value) {
            checkControl.setErrors({ mismatch: true})
            return { mismatch: true };
        } else {
            if (checkControl.hasError('mismatch')) {
                checkControl.setErrors(null);
            }
            return null;
        }
    }
}