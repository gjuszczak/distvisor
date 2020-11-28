import { Component, Inject } from '@angular/core';
import { AuthService } from '../auth.service';
import { BackendDetails } from 'src/api/models';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  
  constructor(
    private authService: AuthService,
    @Inject('BACKEND_DETAILS') public backendDetails: BackendDetails) {    
  }

  login() {
    this.authService.login();
  }
}
