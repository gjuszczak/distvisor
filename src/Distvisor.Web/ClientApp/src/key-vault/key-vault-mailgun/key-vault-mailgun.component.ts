import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-key-vault-mailgun',
  templateUrl: './key-vault-mailgun.component.html'
})
export class KeyVaultMailgunComponent {
  @Output() save: EventEmitter<any> = new EventEmitter();
  inputTo: string;
  inputDomain: string;
  inputKeyValue: string;

  saveClicked() {
    this.save.emit({
      key: this.inputKeyValue,
      to: this.inputTo,
      domain: this.inputDomain,
    });
  }
}
