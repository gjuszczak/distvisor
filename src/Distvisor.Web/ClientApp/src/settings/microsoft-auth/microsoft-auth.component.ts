import { Component, OnInit } from '@angular/core';
import { MicrosoftService } from 'src/api/services';


@Component({
  selector: 'app-microsoft-auth',
  templateUrl: './microsoft-auth.component.html'
})
export class MicrosoftAuthComponent implements OnInit {
  authUri: string;

  constructor(private microsoftService: MicrosoftService) { }

  ngOnInit() {
    this.microsoftService.apiMicrosoftAuthUriGet$Json()
      .subscribe(authParams => {
        this.authUri = authParams.authUri;
      });
  }

  onBackup() {
    this.microsoftService.apiMicrosoftBackupGet()
      .subscribe();
  }
}
