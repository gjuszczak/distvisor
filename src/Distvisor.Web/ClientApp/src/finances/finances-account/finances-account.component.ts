import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FinancialAccountTransactionDto } from 'src/api/models';
import { FinancesService } from 'src/api/services';

@Component({
  selector: 'app-finances-account',
  templateUrl: './finances-account.component.html'
})
export class FinancesAccountComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];

  accountId: string = '';
  isAddAccountTransactionDialogVisible: boolean = false;
  transactions: FinancialAccountTransactionDto[] = [];

  constructor(private financesService: FinancesService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.subscriptions.push(
      this.route.params.subscribe(params => {
        this.accountId = params['id'];
        this.reloadTransactions();
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  reloadTransactions() {
    this.subscriptions.push(
      this.financesService.apiFinancesTransactionsListGet$Json().subscribe(ftran => {
        this.transactions = ftran;
      })
    );
  }

  showAddAccountTransactionDialog() {
    this.isAddAccountTransactionDialogVisible = true;
  }

  hideAddAccountTransactionDialog() {
    this.isAddAccountTransactionDialogVisible = false;
  }
}