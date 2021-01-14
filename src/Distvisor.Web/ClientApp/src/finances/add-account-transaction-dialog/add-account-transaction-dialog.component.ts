import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { FinancesService } from 'src/api/services';

interface AddFinancialAccountTransactionInputs {
  amount?: number;
  balance?: number;
  date?: Date;
  details?: string;
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
      this.financesService.apiFinancesTransactionsAddPost$Response({
        body: {
          accountId: this.accountId,
          amount: this.transaction.amount,
          balance: this.transaction.balance,
          date: this.transaction.date?.toISOString(),
          details: this.transaction.details,
        }
      }).subscribe(
        _ => this.onHide.emit()
      )
    );
  }

  onCancelClicked() {
    this.onHide.emit();
  }

  initFinancialAccountTransaction = () => <AddFinancialAccountTransactionInputs>{
    date: new Date(Date.now()),
  }
}