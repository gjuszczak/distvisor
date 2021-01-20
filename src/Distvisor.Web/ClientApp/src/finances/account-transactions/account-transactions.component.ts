import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FinancialAccountTransactionDto } from 'src/api/models';

@Component({
  selector: 'app-account-transactions',
  templateUrl: './account-transactions.component.html'
})
export class AccountTransactionsComponent {
  @Input() transactions: FinancialAccountTransactionDto[] = [];
  @Output() onAdd: EventEmitter<any> = new EventEmitter();

  cols: any[] = [
    { field: 'transactionDate', header: 'Transaction Date', colStyle: { width: '15%' }, format: 'date' },
    { field: 'postingDate', header: 'Posting Date', colStyle: { width: '15%' }, format: 'date' },
    { field: 'title', header: 'Title', colStyle: { width: '40%' } },
    { field: 'amount', header: 'Amount', colStyle: { width: '15%' } },
    { field: 'balance', header: 'Balance', colStyle: { width: '15%' } }
  ];

  onAddClicked() {
    this.onAdd.emit();
  }
}