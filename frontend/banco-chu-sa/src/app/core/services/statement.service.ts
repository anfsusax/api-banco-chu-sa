import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Statement } from '../../models/statement.model';

@Injectable({
  providedIn: 'root'
})
export class StatementService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7263/api/v1/statements';

  getStatement(accountNumber: string, startDate: Date, endDate: Date): Observable<Statement> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get<Statement>(`${this.apiUrl}/${accountNumber}`, { params });
  }
}

