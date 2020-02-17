import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SelectItem } from 'primeng/api';
import { SecretKey } from '../../api/models/secret-key';
import { SecretsVaultService } from '../../api/services';

@Component({
  selector: 'app-secrets-vault',
  templateUrl: './secrets-vault.component.html'
})
export class SecretsVaultComponent implements OnInit, OnDestroy {

  private subscriptions: Subscription[] = [];
  secretKey = SecretKey;
  allSecretKeys: SelectItem[];
  selectedSecretKey: SecretKey;
  storedSecretKeys: SecretKey[];

  constructor(private secretsVaultService: SecretsVaultService) { }

  ngOnInit() {
    this.allSecretKeys = Object.keys(SecretKey).map(v => <SelectItem>{ label: v, value: v });
    this.selectedSecretKey = this.allSecretKeys[0].value;

    this.reloadList();
  }

  reloadList() {
    this.subscriptions.push(this.secretsVaultService.apiSecretsVaultListGet$Json()
      .subscribe(keys => {
        this.storedSecretKeys = keys;
      }));
  }

  onSave(value: string) {
    this.subscriptions.push(this.secretsVaultService.apiSecretsVaultKeyPost({
      key: this.selectedSecretKey,
      value: value,
    }).subscribe(() => this.reloadList()));
  }

  onRemove(key: SecretKey) {
    this.subscriptions.push(this.secretsVaultService.apiSecretsVaultKeyDelete({
      key: key,
    }).subscribe(() => this.reloadList()));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
