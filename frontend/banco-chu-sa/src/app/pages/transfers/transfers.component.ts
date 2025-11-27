import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { TransferService } from '../../core/services/transfer.service';
import { Transfer } from '../../models/transfer.model';
import { Router } from '@angular/router';
import { ValidationMessageService } from '../../core/services/validation-message.service';
import { differentAccountsValidator, minAmountValidator } from '../../core/validators/custom.validators';
import { getErrorMessage, getSuccessMessage } from '../../core/utils/error.utils';

@Component({
  selector: 'app-transfers',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './transfers.component.html',
  styleUrl: './transfers.component.scss'
})
export class TransfersComponent {
  private transferService = inject(TransferService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  protected validationMessageService = inject(ValidationMessageService);

  transferForm: FormGroup;
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  constructor() {
    this.transferForm = this.fb.group({
      fromAccountNumber: ['', [Validators.required]],
      toAccountNumber: ['', [Validators.required]],
      amount: [0, [Validators.required, minAmountValidator()]],
      description: ['', [Validators.required, Validators.maxLength(500)]]
    }, {
      validators: differentAccountsValidator()
    });
  }

  onSubmit(): void {
    if (this.transferForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';

      const transfer: Transfer = this.transferForm.value;

      this.transferService.executeTransfer(transfer).subscribe({
        next: (response) => {
          this.successMessage = getSuccessMessage(response);
          this.transferForm.reset();
          this.isLoading = false;
        },
        error: (error) => {
          this.errorMessage = getErrorMessage(error);
          this.isLoading = false;
        }
      });
    }
  }

  navigateToAccounts(): void {
    this.router.navigate(['/accounts']);
  }

  navigateToStatements(): void {
    this.router.navigate(['/statements']);
  }

  getFieldError(fieldName: string): string {
    const control = this.transferForm.get(fieldName);
    return this.validationMessageService.getErrorMessage(control, fieldName);
  }

  getFormError(): string {
    return this.validationMessageService.getFormErrorMessage(this.transferForm, 'sameAccounts');
  }
}

