import { Component, OnInit } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { KeyType } from '../key-type';

@Component({
  selector: 'app-key-vault',
  templateUrl: './key-vault.component.html'
})
export class KeyVaultComponent implements OnInit {

  keyTypes: SelectItem[];
  selectedKeyType: string;

  cars: Car[];

  selectedCar: Car;

  cols: any[];

  ngOnInit() {

    this.keyTypes = Object.keys(KeyType).map(v => <SelectItem>{ label: v, value: v });
    this.selectedKeyType = this.keyTypes[0].value;

    this.cars = [
      { vin: 'vin1', year: 'year1', brand: 'brand1', color: 'color1' },
      { vin: 'vin2', year: 'year1', brand: 'brand1', color: 'color1' },
      { vin: 'vin3', year: 'year1', brand: 'brand1', color: 'color1' },
      { vin: 'vin4', year: 'year1', brand: 'brand1', color: 'color1' },
    ];

    this.cols = [
      { field: 'vin', header: 'Vin' },
      { field: 'year', header: 'Year' },
      { field: 'brand', header: 'Brand' },
      { field: 'color', header: 'Color' }
    ];
  }
}

export class Car {
  vin: string;
  year: string;
  brand: string;
  color: string;
}
