import { Component, OnInit, OnDestroy } from '@angular/core';
import { SettingsService } from '../settings.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-updates',
  templateUrl: './updates.component.html'
})
export class UpdatesComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  versions: UpdateVersion[];
  selectedVersion: UpdateVersion;

  constructor(private settingsService: SettingsService) { }

  ngOnInit() {
    this.subscriptions.push(this.settingsService.getUpdates()
      .subscribe(apiVersions => {
        this.versions = apiVersions.map(v => <UpdateVersion>{ tag: v });
        this.selectedVersion = this.versions[0];
      }));
  }

  onUpdate() {
    this.subscriptions.push(this.settingsService.update(this.selectedVersion.tag)
      .subscribe(status => console.log(status)));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}

interface UpdateVersion {
  tag: string;
}
