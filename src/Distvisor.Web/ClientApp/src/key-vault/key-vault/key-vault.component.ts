import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SelectItem } from 'primeng/api';
import { KeyType } from '../../api/models/key-type';
import { KeyVaultService } from '../../api/services';

@Component({
  selector: 'app-key-vault',
  templateUrl: './key-vault.component.html'
})
export class KeyVaultComponent implements OnInit, OnDestroy {

  private subscriptions: Subscription[] = [];
  keyType = KeyType;
  keyTypes: SelectItem[];
  selectedKeyType: KeyType;
  keyList: KeyType[];

  constructor(private keyVaultService: KeyVaultService) { }

  ngOnInit() {
    this.keyTypes = Object.keys(KeyType).map(v => <SelectItem>{ label: v, value: v });
    this.selectedKeyType = this.keyTypes[0].value;

    this.reloadList();
  }

  reloadList() {
    this.subscriptions.push(this.keyVaultService.apiKeyVaultListGet$Json()
      .subscribe(keys => {
        this.keyList = keys;
      }));
  }

  onSave(body: any) {
    this.subscriptions.push(this.keyVaultService.apiKeyVaultKeyTypePost$Json({
      keyType: this.selectedKeyType,
      body: body,
    }).subscribe(() => this.reloadList()));
  }

  onRemove(keyType: KeyType) {
    this.subscriptions.push(this.keyVaultService.apiKeyVaultKeyTypeDelete({
      keyType: keyType,
    }).subscribe(() => this.reloadList()));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
