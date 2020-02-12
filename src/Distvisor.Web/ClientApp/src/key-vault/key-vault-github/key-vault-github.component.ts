import { Component, OnInit, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-key-vault-github',
  templateUrl: './key-vault-github.component.html'
})
export class KeyVaultGithubComponent {
  @Output() save: EventEmitter<any> = new EventEmitter();
  inputKeyValue: string;
  inputRepoOwner: string;
  inputRepoName: string;

  saveClicked() {
    this.save.emit({
      key: this.inputKeyValue,
      repoOwner: this.inputRepoOwner,
      repoName: this.inputRepoName,
    });
  }
}
