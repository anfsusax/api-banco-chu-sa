import { AbstractControl } from '@angular/forms';
import { ValidationMessageService } from '../services/validation-message.service';

export function getValidationErrorMessage(
  control: AbstractControl | null,
  fieldName: string,
  validationService: ValidationMessageService
): string {
  return validationService.getErrorMessage(control, fieldName);
}

