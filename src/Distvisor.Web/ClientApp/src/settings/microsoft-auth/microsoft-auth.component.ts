import { Component, OnInit } from '@angular/core';
import { MicrosoftService } from 'src/api/services';


@Component({
  selector: 'app-microsoft-auth',
  templateUrl: './microsoft-auth.component.html'
})
export class MicrosoftAuthComponent {

  constructor(private microsoftService: MicrosoftService) { }

  onBackup() {
    this.microsoftService.apiMicrosoftBackupGet()
      .subscribe();
  }
}
