import { Component, OnDestroy, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { FinancesService } from 'src/api/services';

@Component({
  selector: 'app-finances',
  templateUrl: './finances.component.html'
})
export class FinancesComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];

  isAddAccountDialogVisible: boolean = false;
  isImportFinancialFilesDialogVisible: boolean = false;

  cols: any[] = [
    { field: 'name', header: 'Account' },
    { field: 'balance', header: 'Balance' },
    { field: 'lastTransaction', header: 'Last transaciton' },
    { field: 'monthlyIncome', header: 'Monthly income' }
  ];
  accounts: any[] = [];

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
        this.accounts = facc.map(x=> <any>{
          name: x.name,
          number: x.number,
          paycards: x.paycards,
          balance: 0,
          lastTransaction: 0,
          monthlyIncome: 0
        });
      })
    );
  }

  onRowSelect(event: any) {

  }

  showAddAccountDialog() {
    this.isAddAccountDialogVisible = true;
  }

  hideAddAccountDialog() {
    this.isAddAccountDialogVisible = false;
  }

  showImportFinancialFilesDialog() {
    this.isImportFinancialFilesDialogVisible = true;
  }

  hideImportFinancialFilesDialog() {
    this.isImportFinancialFilesDialogVisible = false;
  }
}