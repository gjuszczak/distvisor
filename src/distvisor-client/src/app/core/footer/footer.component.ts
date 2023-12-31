import { Component, Inject } from '@angular/core';
import { ClientConfiguration } from 'src/app/api/models';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {

  constructor(
    private authService: AuthService,
    @Inject('CLIENT_CONFIGURATION') public config: ClientConfiguration) {    
  }

  login() {
    this.authService.login();
  }
}
