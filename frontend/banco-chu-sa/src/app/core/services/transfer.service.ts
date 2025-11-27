import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Transfer } from '../../models/transfer.model';

@Injectable({
  providedIn: 'root'
})
export class TransferService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7263/api/v1/transfers';

  executeTransfer(transfer: Transfer): Observable<any> {
    return this.http.post(`${this.apiUrl}`, transfer);
  }
}

