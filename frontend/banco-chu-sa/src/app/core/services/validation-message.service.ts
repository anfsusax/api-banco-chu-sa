import { Injectable } from '@angular/core';
import { AbstractControl, ValidationErrors } from '@angular/forms';

export interface ValidationMessages {
  [key: string]: string | ((error: any) => string);
}

@Injectable({
  providedIn: 'root'
})
export class ValidationMessageService {
  private messages: ValidationMessages = {
    required: 'Este campo é obrigatório',
    email: 'E-mail inválido',
    min: (error: { min: number; actual: number }) => 
      `O valor mínimo permitido é ${error.min}`,
    max: (error: { max: number; actual: number }) => 
      `O valor máximo permitido é ${error.max}`,
    minlength: (error: { requiredLength: number; actualLength: number }) => 
      `O tamanho mínimo é ${error.requiredLength} caracteres`,
    maxlength: (error: { requiredLength: number; actualLength: number }) => 
      `O tamanho máximo é ${error.requiredLength} caracteres`,
    documentNumber: 'CPF/CNPJ é obrigatório (11 ou 14 caracteres)',
    minAmount: 'Valor deve ser maior que zero',
    sameAccounts: 'Conta de origem e destino não podem ser iguais',
    invalidDateRange: 'Data inicial não pode ser maior que data final',
    username: 'Usuário é obrigatório',
    password: 'Senha é obrigatória',
    ownerName: 'Nome é obrigatório',
    accountNumber: 'Número da conta é obrigatório',
    fromAccountNumber: 'Conta de origem é obrigatória',
    toAccountNumber: 'Conta de destino é obrigatória',
    amount: 'Valor deve ser maior que zero',
    description: 'Descrição é obrigatória',
    startDate: 'Data inicial é obrigatória',
    endDate: 'Data final é obrigatória'
  };

  getErrorMessage(control: AbstractControl | null, fieldName?: string): string {
    if (!control || !control.errors || !control.touched) {
      return '';
    }

    const errors = control.errors;
    const firstErrorKey = Object.keys(errors)[0];
    
    if (!firstErrorKey) {
      return '';
    }

    if (fieldName) {
      const fieldSpecificKey = `${fieldName}`;
      if (this.messages[fieldSpecificKey]) {
        return this.getMessage(fieldSpecificKey, errors[firstErrorKey]);
      }
    }

    const errorMessage = this.messages[firstErrorKey];
    if (errorMessage) {
      return this.getMessage(firstErrorKey, errors[firstErrorKey]);
    }

    return 'Campo inválido';
  }

  getFormErrorMessage(form: AbstractControl, errorKey: string): string {
    if (!form.errors || !form.errors[errorKey]) {
      return '';
    }

    const errorMessage = this.messages[errorKey];
    if (errorMessage) {
      return this.getMessage(errorKey, form.errors[errorKey]);
    }

    return 'Formulário inválido';
  }

  private getMessage(key: string, error: any): string {
    const message = this.messages[key];
    
    if (typeof message === 'function') {
      return message(error);
    }
    
    return message || '';
  }

  setMessages(messages: Partial<ValidationMessages>): void {
    const filteredMessages: ValidationMessages = {};
    for (const key in messages) {
      const value = messages[key];
      if (value !== undefined) {
        filteredMessages[key] = value;
      }
    }
    this.messages = { ...this.messages, ...filteredMessages };
  }
}

