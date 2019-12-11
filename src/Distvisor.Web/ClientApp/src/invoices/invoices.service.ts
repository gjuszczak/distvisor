import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Invoice } from './invoices/invoices.component';

@Injectable({
  providedIn: 'root'
})
export class InvoicesService {

  constructor(private http: HttpClient) { }

  getInvoicesLarge() {
    return this.http.get('/api/invoices/list')
      .toPromise()
      .then(res => <Invoice[]>res)
      .then(data => { return data; });
  }
}

