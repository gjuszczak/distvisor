import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FinancialSummaryDataSetDto, FinancialSummaryDto } from 'src/app/api/models';

@Component({
  selector: 'app-accounts-summary',
  templateUrl: './accounts-summary.component.html'
})
export class AccountsSummaryComponent implements OnChanges {
  @Input() summary: FinancialSummaryDto = {};

  data: any;
  options: any;

  constructor() {
    let total = 0;
    this.options = {
      scales: {
        yAxes: [{
          stacked: true,
          ticks: { 
            min: 0,
          },
        }]
      },
      tooltips: {
        mode: 'index',
        intersect: false,
        callbacks: {
          afterTitle: function() {
            total = 0;
          },
          label: function(tooltipItem : any, data: any) {
              var l = data.datasets[tooltipItem.datasetIndex].label;
              var d = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
              total += d;
              return l + ": " + d.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");             
          },
          footer: function() {
              return "TOTAL: " + total.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
          }
        },
      },
    };
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['summary']) {
      this.loadData();
    }
  }

  loadData() {
    const datasets = this.summary.lineChart?.dataSets
      ?.map(ds => this.toChartDataset(ds || {})) || [];

    this.data = {
      labels: this.summary.lineChart?.labels || [],
      datasets: datasets
    }
  }

  toChartDataset(dataset: FinancialSummaryDataSetDto): any {
    return {
      label: dataset.label || "no_label",
      data: dataset.data || [],
      fill: true,
      pointRadius: 0,
      spanGaps: true,
    }
  }
}