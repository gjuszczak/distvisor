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

  onPreviewInvoiceClicked(invoiceId: string) {
    var newWindow = window.open('', '_blank');
    newWindow.document.write('Loading pdf...');
    this.invoicesService.apiInvoicesInvoiceIdGet$Response({ invoiceId: invoiceId })
      .subscribe(resp => {
        const fileURL = URL.createObjectURL(resp.body);
        newWindow.location.href = fileURL;
      },
      _ => {
        newWindow.document.write('Loading failed!');
      });
  }

  onSendMailInvoiceClicked(invoiceId: string) {
    this.invoicesService.apiInvoicesInvoiceIdSendMailPost$Response({ invoiceId: invoiceId })
      .subscribe(_ => {
        console.log("mail-sent");
      },
      _ => {
        console.error("mail-sent-error");
      });
  }
}