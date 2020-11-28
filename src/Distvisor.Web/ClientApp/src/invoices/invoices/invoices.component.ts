import { Component, OnInit } from '@angular/core';
import { InvoicesService } from 'src/api/services';
import { Invoice } from 'src/api/models';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html'
})
export class InvoicesComponent implements OnInit {
  invoices: Invoice[] = [];
  issueDate: Date;
  workdays: number;
  templateInvoices: SelectItem[] = [];
  selectedTemplateInvoiceId: string | null = null;

  constructor(private invoicesService: InvoicesService) {
    this.issueDate = new Date(Date.now());
    this.workdays = 20;
  }

  ngOnInit() {
    this.reloadInvoices();
  }

  reloadInvoices() {
    this.invoicesService.apiInvoicesListGet$Json()
      .subscribe(x => {
        this.invoices = x;
        this.templateInvoices =
          x.map(inv => <SelectItem>{ label: inv.number, value: inv.id });
        this.selectedTemplateInvoiceId = this.templateInvoices[0].value;
      });
  }

  onPreviewInvoiceClicked(invoiceId: string) {
    const newWindow = window.open('', '_blank');
    if (newWindow) {
      newWindow.document.write('Loading pdf...');
      this.invoicesService.apiInvoicesInvoiceIdGet$Response({ invoiceId: invoiceId })
        .subscribe(resp => {
          const fileURL = URL.createObjectURL(resp.body);
          newWindow.location.href = fileURL;
        }, _ => {
          newWindow.document.write('Loading failed!');
        });
    }
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
    if (this.selectedTemplateInvoiceId) {
      this.invoicesService.apiInvoicesGeneratePost$Response({
        body: {
          templateInvoiceId: this.selectedTemplateInvoiceId,
          utcIssueDate: this.issueDate.toISOString(),
          workdays: this.workdays
        }
      }).subscribe(_ => {
        console.log("generated");
        this.reloadInvoices();
      }, _ => {
        console.error("generate-error");
      });
    }
  }
}