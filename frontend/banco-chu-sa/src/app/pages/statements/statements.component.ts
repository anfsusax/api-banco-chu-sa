import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { StatementService } from '../../core/services/statement.service';
import { Statement } from '../../models/statement.model';
import { Router } from '@angular/router';
import { ValidationMessageService } from '../../core/services/validation-message.service';
import { dateRangeValidator } from '../../core/validators/custom.validators';
import { getErrorMessage } from '../../core/utils/error.utils';

@Component({
  selector: 'app-statements',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './statements.component.html',
  styleUrl: './statements.component.scss'
})
export class StatementsComponent {
  private statementService = inject(StatementService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  protected validationMessageService = inject(ValidationMessageService);

  statementForm: FormGroup;
  statement: Statement | null = null;
  isLoading: boolean = false;
  errorMessage: string = '';

  constructor() {
    const today = new Date();
    const firstDayOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);

    this.statementForm = this.fb.group({
      accountNumber: ['', [Validators.required]],
      startDate: [firstDayOfMonth.toISOString().split('T')[0], [Validators.required]],
      endDate: [today.toISOString().split('T')[0], [Validators.required]]
    }, {
      validators: dateRangeValidator()
    });
  }

  onSubmit(): void {
    if (this.statementForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.statement = null;

      const accountNumber = this.statementForm.value.accountNumber;
      const startDate = new Date(this.statementForm.value.startDate);
      const endDate = new Date(this.statementForm.value.endDate);

      this.statementService.getStatement(accountNumber, startDate, endDate).subscribe({
        next: (statement) => {
          this.statement = statement;
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

  navigateToTransfers(): void {
    this.router.navigate(['/transfers']);
  }

  getFieldError(fieldName: string): string {
    const control = this.statementForm.get(fieldName);
    return this.validationMessageService.getErrorMessage(control, fieldName);
  }

  getFormError(): string {
    return this.validationMessageService.getFormErrorMessage(this.statementForm, 'invalidDateRange');
  }
}

