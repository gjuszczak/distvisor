import { Component, OnInit } from '@angular/core';
import { NavMenuService } from '../nav-menu/nav-menu.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent  implements OnInit {

  constructor(private navMenuService: NavMenuService) { }

  ngOnInit(){
    this.navMenuService.setNavBrand(null);
  }
}
