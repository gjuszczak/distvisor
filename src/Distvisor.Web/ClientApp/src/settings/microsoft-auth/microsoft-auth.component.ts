import { Component, OnInit} from '@angular/core';
import { MicrosoftService } from 'src/api/services';


@Component({
  selector: 'app-microsoft-auth',
  templateUrl: './microsoft-auth.component.html'
})
export class MicrosoftAuthComponent implements OnInit {
  authUrl: string;

  constructor(private microsoftService: MicrosoftService) { }

  ngOnInit() {
    this.microsoftService.apiMicrosoftAuthUrlGet$Json()
    .subscribe(authParams => {
      this.authUrl = authParams.authUrl;
    });
  }
}
