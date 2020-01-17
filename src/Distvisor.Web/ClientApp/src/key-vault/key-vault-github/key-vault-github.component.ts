import { Component, OnInit, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-key-vault-github',
  templateUrl: './key-vault-github.component.html'
})
export class KeyVaultGithubComponent {
  @Output() save: EventEmitter<any> = new EventEmitter();
  inputKeyValue: string;

  saveClicked() {
    this.save.emit({ key: this.inputKeyValue });
  }
}
