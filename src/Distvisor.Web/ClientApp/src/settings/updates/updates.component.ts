import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AdminService } from 'src/api/services';
import { SelectItem } from 'primeng/api';
import { UpdateRequestDto } from 'src/api/models/update-request-dto';

@Component({
  selector: 'app-updates',
  templateUrl: './updates.component.html'
})
export class UpdatesComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  versions: SelectItem[];
  selectedVersion: string;
  dbUpdateStrategies: SelectItem[];
  selectedDbUpdateStrategy: string;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.subscriptions.push(this.adminService.apiAdminUpdateParamsGet$Json()
      .subscribe(updateParams => {
        this.versions = updateParams.versions.map(v => <SelectItem>{ label: v, value: v });
        this.selectedVersion = this.versions[0].value;
        this.dbUpdateStrategies = updateParams.dbUpdateStrategies.map(v => <SelectItem>{ label: v, value: v });
        this.selectedDbUpdateStrategy = this.dbUpdateStrategies[0].value;
      }));
  }

  onUpdate() {
    this.subscriptions.push(this.adminService.apiAdminUpdatePost$Json({
      body: <UpdateRequestDto>{
        updateToVersion: this.selectedVersion,
        dbUpdateStrategy: this.selectedDbUpdateStrategy,
    }}).subscribe());
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
