import { Component, Inject } from '@angular/core';
import { BackendDetails } from 'src/app/api/models';
import { AuthService } from '../services/auth.service';

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
