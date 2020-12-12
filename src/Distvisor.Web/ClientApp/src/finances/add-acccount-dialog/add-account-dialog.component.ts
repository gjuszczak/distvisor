import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { FinancialAccount, FinancialAccountType } from 'src/api/models';
import { FinancesService } from 'src/api/services';

@Component({
  selector: 'app-add-account-dialog',
  templateUrl: './add-account-dialog.component.html'
})
export class AddAccountDialogComponent implements OnChanges, OnDestroy {
  @Input() isVisible: boolean = false;
  @Output() onHide: EventEmitter<any> = new EventEmitter();

  private subscriptions: Subscription[] = [];

  account: FinancialAccount;

  constructor(private financesService: FinancesService) {
    this.account = this.initFinancialAccount();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isVisible']) {
      this.account = this.initFinancialAccount();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  onSaveClicked() {
    this.subscriptions.push(
      this.financesService.apiFinancesAccountsAddPost$Response({
        body: this.account
      }).subscribe(
        _ => this.onHide.emit()
      )
    );
  }

  onCancelClicked() {
    this.onHide.emit();
  }

  initFinancialAccount = () => <FinancialAccount>{
    name: '',
    number: '',
    paycards: [],
    type: FinancialAccountType.Bank,
  }
}