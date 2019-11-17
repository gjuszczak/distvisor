import { Component, OnInit } from '@angular/core';
import { NavigationService } from '../../navigation/navigation.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent  implements OnInit {

  constructor(private navigationService: NavigationService) { }

  ngOnInit(){
    this.navigationService.setNavBrand(null);
  }
}
