import { Component } from '@angular/core';
import { NavigationService } from 'src/navigation/navigation.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html'
})
export class SettingsComponent {

  constructor(private nav: NavigationService) { }

  ngOnInit() {
    this.nav.setNavBrand("Settings");
  }
}
