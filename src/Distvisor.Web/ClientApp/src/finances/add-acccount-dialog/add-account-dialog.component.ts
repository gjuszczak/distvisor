import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-add-account-dialog',
  templateUrl: './add-account-dialog.component.html'
})
export class AddAccountDialogComponent {
  @Input() isVisible: boolean = false;
  @Output() onSave: EventEmitter<any> = new EventEmitter();
  @Output() onCancel: EventEmitter<any> = new EventEmitter();

  onSaveClicked() {
    this.onSave.emit();
  }
  
  onCancelClicked() {
    this.onCancel.emit();
  }
}