import { Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { FinancialAccount } from 'src/api/models';
import { FinancesService } from 'src/api/services';

@Component({
  selector: 'app-add-account-dialog',
  templateUrl: './add-account-dialog.component.html'
})
export class AddAccountDialogComponent implements OnDestroy {
  @Input() isVisible: boolean = false;
  @Output() onHide: EventEmitter<any> = new EventEmitter();

  private subscriptions: Subscription[] = [];

  account: FinancialAccount = {
    name: "",
    number: "",
    paycards: []
  };

  constructor(private financesService: FinancesService) { }

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
}