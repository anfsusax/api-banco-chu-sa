import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account.service';
import { Account, CreateAccount } from '../../models/account.model';
import { Router } from '@angular/router';
import { ValidationMessageService } from '../../core/services/validation-message.service';
import { documentNumberValidator } from '../../core/validators/custom.validators';
import { getErrorMessage, getSuccessMessage } from '../../core/utils/error.utils';

@Component({
  selector: 'app-accounts',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './accounts.component.html',
  styleUrl: './accounts.component.scss'
})
export class AccountsComponent implements OnInit {
  private accountService = inject(AccountService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  protected validationMessageService = inject(ValidationMessageService);

  accounts: Account[] = [];
  accountForm: FormGroup;
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  constructor() {
    this.accountForm = this.fb.group({
      ownerName: ['', [Validators.required, Validators.maxLength(200)]],
      documentNumber: ['', [Validators.required, documentNumberValidator()]],
      initialBalance: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
  }

  onCreateAccount(): void {
    if (this.accountForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      this.successMessage = '';

      const createAccount: CreateAccount = {
        ownerName: this.accountForm.value.ownerName,
        documentNumber: this.accountForm.value.documentNumber,
        initialBalance: this.accountForm.value.initialBalance
      };

      this.accountService.createAccount(createAccount).subscribe({
        next: (response) => {
          this.successMessage = getSuccessMessage(response);
          this.accountForm.reset({ initialBalance: 0 });
          this.isLoading = false;
        },
        error: (error) => {
          this.errorMessage = getErrorMessage(error);
          this.isLoading = false;
        }
      });
    }
  }

  onSearchAccount(accountNumber: string): void {
    if (accountNumber) {
      this.isLoading = true;
      this.accountService.getAccountByNumber(accountNumber).subscribe({
        next: (account) => {
          this.accounts = [account];
          this.isLoading = false;
        },
        error: (error) => {
          this.errorMessage = getErrorMessage(error);
          this.accounts = [];
          this.isLoading = false;
        }
      });
    }
  }

  navigateToTransfers(): void {
    this.router.navigate(['/transfers']);
  }

  navigateToStatements(): void {
    this.router.navigate(['/statements']);
  }

  getFieldError(fieldName: string): string {
    const control = this.accountForm.get(fieldName);
    return this.validationMessageService.getErrorMessage(control, fieldName);
  }
}

