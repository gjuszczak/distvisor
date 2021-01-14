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
    { field: 'date', header: 'Date', isDate: true },
    { field: 'details', header: 'Details' },
    { field: 'amount', header: 'Amount' },
    { field: 'balance', header: 'Balance' }
  ];

  onAddClicked() {
    this.onAdd.emit();
  }
}