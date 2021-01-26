import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FinancialSummaryDto } from 'src/api/models';

@Component({
  selector: 'app-accounts-summary',
  templateUrl: './accounts-summary.component.html'
})
export class AccountsSummaryComponent implements OnChanges {
  @Input() summary: FinancialSummaryDto = { lineChart: { labels: [], dataSets: [] } };

  data: any;

  constructor() {
    this.data = {
      labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
      datasets: [
        {
          label: 'First Dataset',
          data: [65, 59, 80, 81, 56, 55, 40],
          fill: false,
          borderColor: '#4bc0c0'
        },
        {
          label: 'Second Dataset',
          data: [28, 48, 40, 19, 86, 27, 90],
          fill: false,
          borderColor: '#565656'
        }
      ]
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['summary']) {
      this.loadData();
    }
  }

  loadData() {
    this.data = {
      labels: this.summary.lineChart?.labels || [],
      datasets: this.summary.lineChart?.dataSets?.map(ds => <any>{
        label: ds.label || "no_label",
        data: ds.data || [],
        fill: false,
        borderColor: '#4bc0c0'
      }) || []
    }
  }
}