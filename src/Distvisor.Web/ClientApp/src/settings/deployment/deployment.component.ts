import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SelectItem } from 'primeng/api';
import { AdminService } from 'src/api/services';
import { DeployRequestDto, RedeployRequestDto } from 'src/api/models';

@Component({
  selector: 'app-deployment',
  templateUrl: './deployment.component.html'
})
export class DeploymentComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  environments: SelectItem[];
  selectedEnvironment: string;
  versions: SelectItem[];
  selectedVersion: string;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.subscriptions.push(this.adminService.apiAdminDeploymentParamsGet$Json()
      .subscribe(deployParams => {
        this.environments = deployParams.environments.map(v => <SelectItem>{ label: v, value: v });
        this.selectedEnvironment = this.environments[0].value;
        this.versions = deployParams.versions.map(v => <SelectItem>{ label: v, value: v });
        this.selectedVersion = null;
      }));
  }

  onSubmit() {
    if (this.selectedVersion) {
      this.subscriptions.push(this.adminService.apiAdminDeployPost({
        body: <DeployRequestDto>{
          environment: this.selectedEnvironment,
          version: this.selectedVersion,
        }
      }).subscribe());
    }
    else {
      this.subscriptions.push(this.adminService.apiAdminRedeployPost({
        body: <RedeployRequestDto>{
          environment: this.selectedEnvironment,
        }
      }).subscribe());
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
