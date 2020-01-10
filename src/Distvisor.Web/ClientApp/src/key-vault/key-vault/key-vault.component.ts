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
  keyTypes: SelectItem[];
  selectedKeyType: KeyType;
  inputKeyValue: string;
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

  onSave() {
    this.subscriptions.push(this.keyVaultService.apiKeyVaultKeyTypePost$Json({
      keyType: this.selectedKeyType,
      body: {
        keyValue: this.inputKeyValue
      }
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
