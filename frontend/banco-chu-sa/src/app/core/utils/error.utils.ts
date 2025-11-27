import { HttpErrorResponse } from '@angular/common/http';

export function getErrorMessage(error: unknown): string {
  if (error instanceof HttpErrorResponse) {
    const errorObj = error.error;
    
    if (typeof errorObj === 'string') {
      return errorObj;
    }
    
    if (errorObj && typeof errorObj === 'object') {
      return (
        errorObj.error ||
        errorObj.message ||
        errorObj.title ||
        errorObj.detail ||
        error.error?.message ||
        ''
      );
    }
  }
  
  return '';
}

export function getSuccessMessage(response: any): string {
  if (!response) {
    return '';
  }
  
  if (typeof response === 'string') {
    return response;
  }
  
  if (typeof response === 'object') {
    return (
      response.message ||
      response.successMessage ||
      response.success ||
      ''
    );
  }
  
  return '';
}

