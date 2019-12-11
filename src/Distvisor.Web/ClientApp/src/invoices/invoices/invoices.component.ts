import { Component, OnInit } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { InvoicesService } from '../invoices.service';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html'
})
export class InvoicesComponent implements OnInit {

  invoices: Invoice[];

  selectedInvoice: Invoice;

  displayDialog: boolean;

  sortOptions: SelectItem[];

  sortKey: string;

  sortField: string;

  sortOrder: number;

  constructor(private invoiceService: InvoicesService) { }

  ngOnInit() {
    this.invoiceService.getInvoicesLarge().then(invoices => this.invoices = invoices);

    this.sortOptions = [
      { label: 'Newest First', value: '!year' },
      { label: 'Oldest First', value: 'year' },
      { label: 'Brand', value: 'brand' }
    ];
  }

  selectInvoice(event: Event, invoice: Invoice) {
    this.selectedInvoice = invoice;
    this.displayDialog = true;
    event.preventDefault();
  }

  onSortChange(event) {
    let value = event.value;

    if (value.indexOf('!') === 0) {
      this.sortOrder = -1;
      this.sortField = value.substring(1, value.length);
    }
    else {
      this.sortOrder = 1;
      this.sortField = value;
    }
  }

  onDialogHide() {
    this.selectedInvoice = null;
  }
}

export interface Invoice {
  vin;
  year;
  brand;
  color;
}
