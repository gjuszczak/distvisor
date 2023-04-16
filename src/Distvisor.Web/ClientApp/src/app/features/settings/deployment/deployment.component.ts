import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SelectItem } from 'primeng/api';
import { AdminDeprService } from 'src/app/api/services';
import { DeployRequestDto, RedeployRequestDto } from 'src/app/api/models';

@Component({
  selector: 'app-deployment',
  templateUrl: './deployment.component.html'
})
export class DeploymentComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  environments: SelectItem[] = [];
  selectedEnvironment: string | null = null;
  versions: SelectItem[] = [];
  selectedVersion: string | null = null;

  constructor(private adminService: AdminDeprService) { }

  ngOnInit() {
    this.subscriptions.push(this.adminService.apiSecAdminDeploymentParamsGet$Json()
      .subscribe(deployParams => {
        if (deployParams.environments) {
          this.environments = deployParams.environments.map(v => <SelectItem>{ label: v, value: v });
          this.selectedEnvironment = this.environments[0].value;
        }
        if (deployParams.versions) {
          this.versions = deployParams.versions.map(v => <SelectItem>{ label: v, value: v });
        }
        this.selectedVersion = null;
      }));
  }

  onSubmit() {
    if (this.selectedVersion) {
      this.subscriptions.push(this.adminService.apiSecAdminDeployPost({
        body: <DeployRequestDto>{
          environment: this.selectedEnvironment,
          version: this.selectedVersion,
        }
      }).subscribe());
    }
    else {
      this.subscriptions.push(this.adminService.apiSecAdminRedeployPost({
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
