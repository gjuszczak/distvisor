import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { FinancialAccount } from 'src/api/models';
import { FinancesService } from 'src/api/services';

@Component({
  selector: 'app-account-transactions',
  templateUrl: './account-transactions.component.html'
})
export class AccountTransactionsComponent implements OnInit, OnDestroy {
  @Input() selectedAccount: FinancialAccount | null = null;
  
  private subscriptions: Subscription[] = [];

  isAddAccountDialogVisible: boolean = false;
  isImportFinancialFilesDialogVisible: boolean = false;


  cols: any[] = [
    { field: 'date', header: 'Date' },
    { field: 'details', header: 'Details' },
    { field: 'ammount', header: 'Amount' },
    { field: 'balance', header: 'Balance' }
  ];
  transactions: any[] = [];

  constructor(private financesService: FinancesService) { }

  ngOnInit(): void {
    this.reloadFinances();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  reloadFinances() {
    this.subscriptions.push(
      

      // this.financesService.apiFinancesAccountsListGet$Json().subscribe(facc => {
      //   this.accounts = facc.map(x=> <any>{
      //     name: x.name,
      //     number: x.number,
      //     paycards: x.paycards,
      //     balance: 0,
      //     lastTransaction: 0,
      //     monthlyIncome: 0
      //   });
      // })
    );
  }

  onRowSelect(event: any) {

  }

  showAddAccountDialog() {
    this.isAddAccountDialogVisible = true;
  }

  hideAddAccountDialog() {
    this.isAddAccountDialogVisible = false;
  }

  showImportFinancialFilesDialog() {
    this.isImportFinancialFilesDialogVisible = true;
  }

  hideImportFinancialFilesDialog() {
    this.isImportFinancialFilesDialogVisible = false;
  }
}