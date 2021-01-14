import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FinancialAccountDto } from 'src/api/models';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html'
})
export class AccountsComponent {
  @Input() accounts: FinancialAccountDto[] = [];
  @Output() onAdd: EventEmitter<any> = new EventEmitter();
  @Output() onUpload: EventEmitter<any> = new EventEmitter();
  @Output() onCheck: EventEmitter<any> = new EventEmitter();

  cols: any[] = [
    { field: 'name', header: 'Account' },
    { field: 'type', header: 'Type' },
    { field: 'number', header: 'Number' }
  ];

  onAddClicked() {
    this.onAdd.emit();
  }

  onUploadClicked() {
    this.onUpload.emit();
  }

  onCheckClicked() {
    this.onCheck.emit();
  }
}