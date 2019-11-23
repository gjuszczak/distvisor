import { Component } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { SettingsService } from '../settings.service';

@Component({
  selector: 'app-updates',
  templateUrl: './updates.component.html'
})
export class UpdatesComponent {

  versions: UpdateVersion[];

  selectedVersion: UpdateVersion;

  constructor(private settingsService: SettingsService) {
    this.settingsService.getUpdates().subscribe(apiVersions => {
      this.versions = apiVersions.map(v => <UpdateVersion>{ tag: v });
      this.selectedVersion = this.versions[0];
    });
  }

  onUpdate(){
    this.settingsService.update(this.selectedVersion.tag).subscribe(status => {
      console.log(status);
    });
  }
}

interface UpdateVersion {
  tag: string;
}
