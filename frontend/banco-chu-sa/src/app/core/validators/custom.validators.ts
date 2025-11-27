import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function minAmountValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) {
      return null;
    }

    const value = parseFloat(control.value);
    if (isNaN(value) || value <= 0) {
      return { minAmount: { value: control.value } };
    }

    return null;
  };
}

export function differentAccountsValidator(): ValidatorFn {
  return (form: AbstractControl): ValidationErrors | null => {
    const fromAccount = form.get('fromAccountNumber')?.value;
    const toAccount = form.get('toAccountNumber')?.value;

    if (fromAccount && toAccount && fromAccount === toAccount) {
      return { sameAccounts: true };
    }

    return null;
  };
}

export function dateRangeValidator(): ValidatorFn {
  return (form: AbstractControl): ValidationErrors | null => {
    const startDate = form.get('startDate')?.value;
    const endDate = form.get('endDate')?.value;

    if (startDate && endDate) {
      const start = new Date(startDate);
      const end = new Date(endDate);

      if (start > end) {
        return { invalidDateRange: { startDate, endDate } };
      }
    }

    return null;
  };
}

export function documentNumberValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) {
      return null;
    }

    const value = control.value.replace(/\D/g, '');
    const length = value.length;

    if (length !== 11 && length !== 14) {
      return { documentNumber: { actualLength: length, requiredLength: [11, 14] } };
    }

    return null;
  };
}

