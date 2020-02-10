import { Component, OnInit } from '@angular/core';
import { InvoicesService } from 'src/api/services';
import { Invoice } from 'src/api/models';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html'
})
export class InvoicesComponent implements OnInit {
  invoices: Invoice[];
  issueDate: Date;
  workdays: Number;

  constructor(private invoicesService: InvoicesService) { }

  ngOnInit() {
    this.issueDate = new Date(Date.now());
    this.workdays = 20;

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

  onSubmit(invoiceId: string) {
    this.invoicesService.apiInvoicesGeneratePost$Response({ templateInvoiceId: invoiceId })
      .subscribe(_ => {
        console.log("generated");
      },
      _ => {
        console.error("generate-error");
      });
  }
}