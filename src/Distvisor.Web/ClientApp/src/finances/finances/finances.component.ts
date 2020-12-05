import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-finances',
  templateUrl: './finances.component.html'
})
export class FinancesComponent {
  uploadedFiles: any[] = [];

  isAddAccountDialogVisible: boolean = false;

  cols: any[] = [
    { field: 'accnum', header: 'Account' },
    { field: 'balance', header: 'Balance' },
    { field: 'lastTransaction', header: 'Last transaciton' },
    { field: 'monthlyIncome', header: 'Monthly income' }
  ];
  accountSummaries: any[] = [
    { accnum: '123456789', balance: 1234.44, lastTransaction: -100, monthlyIncome: 3000 },
    { accnum: '123456789', balance: 1234.44, lastTransaction: -100, monthlyIncome: 3000 },
    { accnum: '123456789', balance: 1234.44, lastTransaction: -100, monthlyIncome: 3000 }
  ];

  constructor(private messageService: MessageService) { }

  onUpload(event: any) {
    for (let file of event.files) {
      this.uploadedFiles.push(file);
    }

    this.messageService.add({ severity: 'info', summary: 'File Uploaded', detail: '' });
  }

  showAddAccountDialog() { 
    this.isAddAccountDialogVisible = true;
  }

  hideAddAccountDialog() {
    this.isAddAccountDialogVisible = false;
  }
}