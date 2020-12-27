import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { FinancialAccountDto } from 'src/api/models';
import { FinancesService } from 'src/api/services';

@Component({
  selector: 'app-account-transactions',
  templateUrl: './account-transactions.component.html'
})
export class AccountTransactionsComponent implements OnInit, OnDestroy {
  @Input() selectedAccount: FinancialAccountDto | null = null;

  private subscriptions: Subscription[] = [];

  isAddAccountTransactionDialogVisible: boolean = false;

  cols: any[] = [
    { field: 'date', header: 'Date' },
    { field: 'details', header: 'Details' },
    { field: 'ammount', header: 'Amount' },
    { field: 'balance', header: 'Balance' }
  ];
  transactions: any[] = [];

  constructor(private financesService: FinancesService) { }

  ngOnInit(): void {
    this.reloadTransactions();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  reloadTransactions() {
    // this.subscriptions.push(


    //   this.financesService.apiFinancesAccountsListGet$Json().subscribe(facc => {
    //     this.accounts = facc.map(x=> <any>{
    //       name: x.name,
    //       number: x.number,
    //       paycards: x.paycards,
    //       balance: 0,
    //       lastTransaction: 0,
    //       monthlyIncome: 0
    //     });
    //   })
    // );
  }


  showAddAccountTransactionDialog() {
    this.isAddAccountTransactionDialogVisible = true;
  }

  hideAddAccountTransactionDialog() {
    this.isAddAccountTransactionDialogVisible = false;
  }
}