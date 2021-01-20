import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { FinancesService } from 'src/api/services';

interface AddFinancialAccountTransactionInputs {
  transactionDate: Date;
  postingDate: Date;
  title: string;
  amount: number;
  balance: number;
}

@Component({
  selector: 'app-add-account-transaction-dialog',
  templateUrl: './add-account-transaction-dialog.component.html'
})
export class AddAccountTransactionDialogComponent implements OnChanges, OnDestroy {
  @Input() accountId: string = '';
  @Input() isVisible: boolean = false;
  @Output() onHide: EventEmitter<any> = new EventEmitter();

  private subscriptions: Subscription[] = [];

  transaction: AddFinancialAccountTransactionInputs;

  constructor(private financesService: FinancesService) {
    this.transaction = this.initFinancialAccountTransaction();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isVisible']) {
      this.transaction = this.initFinancialAccountTransaction();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  onSaveClicked() {
    this.subscriptions.push(
      this.financesService.apiFinancesAccountsTransactionsAddPost$Response({
        body: {
          accountId: this.accountId,
          transactionDate: this.transaction.transactionDate.toISOString(),
          postingDate: this.transaction.postingDate.toISOString(),
          title: this.transaction.title,
          amount: this.transaction.amount,
          balance: this.transaction.balance,
        }
      }).subscribe(
        _ => this.onCancelClicked()
      )
    );
  }

  onCancelClicked() {
    if (this.isVisible) {
      this.onHide.emit();
    }
  }

  initFinancialAccountTransaction = () => <AddFinancialAccountTransactionInputs>{
    transactionDate: new Date(Date.now()),
    postingDate: new Date(Date.now()),
    title: '',
  }
}