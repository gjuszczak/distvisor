import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html'
})
export class InvoicesComponent implements OnInit {
  invoices : Invoice[];

  ngOnInit() {
    this.invoices = [
      { identifier: "FV/20/01", customer: "Intel", issueDate: new Date("2020-12-17"), workDays: 20, amount: 5000 },
      { identifier: "FV/20/02", customer: "Intel", issueDate: new Date("2020-12-17"), workDays: 20, amount: 5000 },
      { identifier: "FV/20/03", customer: "Intel", issueDate: new Date("2020-12-17"), workDays: 20, amount: 5000 },
      { identifier: "FV/20/04", customer: "Intel", issueDate: new Date("2020-12-17"), workDays: 20, amount: 5000 },
    ];
  }
}

export interface Invoice {
  identifier : String;
  customer : String;
  issueDate : Date;
  workDays : Number;
  amount : Number;
}