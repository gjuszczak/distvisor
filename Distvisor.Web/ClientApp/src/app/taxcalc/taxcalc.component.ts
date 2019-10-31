import { Component, OnInit } from '@angular/core';
import { NavMenuService } from '../nav-menu/nav-menu.service';

@Component({
  selector: 'app-taxcalc',
  templateUrl: './taxcalc.component.html',
})
export class TaxCalcComponent implements OnInit {

  constructor(private navMenuService: NavMenuService) { }

  ngOnInit(){
    this.navMenuService.setNavBrand("TaxCalc");
  }
}
