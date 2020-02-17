import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-secret-value',
  templateUrl: './secret-value.component.html'
})
export class SecretValueComponent {
  @Output() save: EventEmitter<string> = new EventEmitter();
  inputSecretValue: string;

  saveClicked() {
    this.save.emit(this.inputSecretValue);
  }
}
