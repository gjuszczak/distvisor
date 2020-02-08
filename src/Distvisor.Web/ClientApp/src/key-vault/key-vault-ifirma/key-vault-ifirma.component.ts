import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-key-vault-ifirma',
  templateUrl: './key-vault-ifirma.component.html'
})
export class KeyVaultIfirmaComponent {
  @Output() save: EventEmitter<any> = new EventEmitter();
  inputUsername: string;
  inputInvoiceKeyValue: string;
  inputSubscriberKeyValue: string;

  saveClicked() {
    this.save.emit({
      invoiceKey: this.inputInvoiceKeyValue,
      subscriberKey: this.inputSubscriberKeyValue,
      user: this.inputUsername,
    });
  }
}
