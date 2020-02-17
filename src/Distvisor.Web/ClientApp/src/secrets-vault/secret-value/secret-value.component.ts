import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-secret-value',
  templateUrl: './secret-value.component.html'
})
export class SecretValueComponent {
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
