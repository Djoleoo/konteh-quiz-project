import { AbstractControl, FormArray, FormGroup, UntypedFormGroup, ValidationErrors } from "@angular/forms";
import { IValidationProblemDetails } from "./api/api-reference";

export const FORM_ERROR_KEY = 'general';
export const FORM_FIELD_ERROR_KEY = 'formServer';

const getFormControl = (formControl: AbstractControl | null | undefined, keys: string[]): AbstractControl | null | undefined => {
    if (keys.length === 0) {
        return formControl;
    }

    if (formControl instanceof FormGroup) {
        return getFormControl(formControl.controls[keys[0].charAt(0).toLocaleLowerCase() + keys[0].slice(1)], keys.slice(1));
    }

    if (formControl instanceof FormArray && +keys[0] >= 0) {
        return getFormControl(formControl.controls[+keys[0]], keys.slice(1));
    }

    return null;
}

export const setServerSideValidationErrors = (error: IValidationProblemDetails, form: UntypedFormGroup) => {
    form.setErrors(null);
    const validationErrors = error?.errors;
    const formErrors: ValidationErrors = {};

    if (validationErrors) {
        for (const key in validationErrors) {
            const control = getFormControl(form, key.split(/[.[\]]+/gu));

            if (control) {
                const fieldErrors = {} as ValidationErrors;
                fieldErrors[FORM_FIELD_ERROR_KEY] = validationErrors[key];
                control.setErrors(fieldErrors);
                control.markAsTouched();
            } else {
                formErrors[FORM_ERROR_KEY] = formErrors[FORM_ERROR_KEY]?.concat(validationErrors[key]) || validationErrors[key];
            }
        }
    }else{
        formErrors[FORM_ERROR_KEY] = ['An error occurred on the server'];
    }
    form.setErrors(formErrors);
}