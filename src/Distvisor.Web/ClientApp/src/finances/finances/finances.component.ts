import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { FinancialAccountDto } from 'src/api/models';
import { FinancesService } from 'src/api/services';

@Component({
  selector: 'app-finances',
  templateUrl: './finances.component.html'
})
export class FinancesComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];

  isAddAccountDialogVisible: boolean = false;
  isImportFinancialFilesDialogVisible: boolean = false;
  accounts: FinancialAccountDto[] = [];

  constructor(private financesService: FinancesService) { }

  ngOnInit(): void {
    this.reloadFinances();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  reloadFinances() {
    this.subscriptions.push(
      this.financesService.apiFinancesAccountsListGet$Json().subscribe(facc => {
        this.accounts = facc;
      })
    );
  }

  showAddAccountDialog() {
    this.isAddAccountDialogVisible = true;
  }

  hideAddAccountDialog() {
    this.isAddAccountDialogVisible = false;
    this.reloadFinances();
  }

  showImportFinancialFilesDialog() {
    this.isImportFinancialFilesDialogVisible = true;
  }

  hideImportFinancialFilesDialog() {
    this.isImportFinancialFilesDialogVisible = false;
    this.reloadFinances();
  }
}