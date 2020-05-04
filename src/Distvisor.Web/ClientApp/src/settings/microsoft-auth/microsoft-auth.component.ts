import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/api/services';


@Component({
  selector: 'app-microsoft-auth',
  templateUrl: './microsoft-auth.component.html'
})
export class MicrosoftAuthComponent {

  constructor(private adminService: AdminService) { }

  onBackup() {
    this.adminService.apiAdminBackupPost()
      .subscribe();
  }
}
