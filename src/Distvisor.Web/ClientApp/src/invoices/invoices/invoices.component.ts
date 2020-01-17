import { Component, OnInit } from '@angular/core';
import { InvoicesService } from 'src/api/services';
import { Invoice } from 'src/api/models';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html'
})
export class InvoicesComponent implements OnInit {
  invoices: Invoice[];

  constructor(private invoicesService: InvoicesService) { }

  ngOnInit() {
    this.invoicesService.apiInvoicesListGet$Json()
      .subscribe(x => this.invoices = x);
  }
}