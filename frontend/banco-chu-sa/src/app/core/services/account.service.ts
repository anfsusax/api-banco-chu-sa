import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Account, CreateAccount } from '../../models/account.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7263/api/v1/accounts';

  createAccount(account: CreateAccount): Observable<Account> {
    return this.http.post<Account>(this.apiUrl, account);
  }

  getAccountByNumber(accountNumber: string): Observable<Account> {
    return this.http.get<Account>(`${this.apiUrl}/${accountNumber}`);
  }
}

