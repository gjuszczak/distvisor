import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-key-vault-ifirma',
  templateUrl: './key-vault-ifirma.component.html'
})
export class KeyVaultIfirmaComponent {
  @Output() save: EventEmitter<any> = new EventEmitter();
  inputUsername: string;
  inputKeyValue: string;

  saveClicked() {
    this.save.emit({ key: this.inputKeyValue, user: this.inputUsername });
  }
}
