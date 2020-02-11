import { Component, OnInit } from '@angular/core';
import { InvoicesService } from 'src/api/services';
import { Invoice } from 'src/api/models';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html'
})
export class InvoicesComponent implements OnInit {
  invoices: Invoice[];
  issueDate: Date;
  workdays: number;
  templateInvoices: SelectItem[];
  selectedTemplateInvoice: SelectItem;

  constructor(private invoicesService: InvoicesService) { }

  ngOnInit() {
    this.issueDate = new Date(Date.now());
    this.workdays = 20;

    this.reloadInvoices();
  }

  reloadInvoices() {
    this.invoicesService.apiInvoicesListGet$Json()
      .subscribe(x => {
        this.invoices = x;
        this.templateInvoices =
          x.map(inv => <SelectItem>{ label: inv.number, value: inv.id });
        this.selectedTemplateInvoice = this.templateInvoices[0];
      });
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

  onSubmit() {
    this.invoicesService.apiInvoicesGeneratePost$Json$Response({
      body: {
        templateInvoiceId: <string>this.selectedTemplateInvoice.value,
        utcIssueDate: this.issueDate.toISOString(),
        workdays: this.workdays
      }
    })
      .subscribe(_ => {
        console.log("generated");
        this.reloadInvoices();
      },
        _ => {
          console.error("generate-error");
        });
  }
}