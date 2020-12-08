import { Component, OnDestroy, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { FinancialAccount } from 'src/api/models';
import { FinancesService } from 'src/api/services';

@Component({
  selector: 'app-finances',
  templateUrl: './finances.component.html'
})
export class FinancesComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];

  uploadedFiles: any[] = [];

  isAddAccountDialogVisible: boolean = false;

  cols: any[] = [
    { field: 'name', header: 'Account' },
    { field: 'balance', header: 'Balance' },
    { field: 'lastTransaction', header: 'Last transaciton' },
    { field: 'monthlyIncome', header: 'Monthly income' }
  ];
  accounts: any[] = [];

  constructor(private messageService: MessageService, private financesService: FinancesService) { }

  ngOnInit(): void {
    this.reloadFinances();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  onUpload(event: any) {
    for (let file of event.files) {
      this.uploadedFiles.push(file);
    }

    this.messageService.add({ severity: 'info', summary: 'File Uploaded', detail: '' });
  }

  reloadFinances() {
    this.subscriptions.push(
      this.financesService.apiFinancesAccountsListGet$Json().subscribe(facc => {
        this.accounts = facc.map(x=> <any>{
          name: x.name,
          number: x.number,
          paycards: x.paycards,
          balance: 0,
          lastTransaction: 0,
          monthlyIncome: 0
        });
      })
    );
  }

  showAddAccountDialog() {
    this.isAddAccountDialogVisible = true;
  }

  hideAddAccountDialog() {
    this.isAddAccountDialogVisible = false;
  }
}