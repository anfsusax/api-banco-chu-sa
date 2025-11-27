import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Transfer } from '../../models/transfer.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TransferService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/transfers`;

  executeTransfer(transfer: Transfer): Observable<any> {
    return this.http.post(`${this.apiUrl}`, transfer);
  }
}

