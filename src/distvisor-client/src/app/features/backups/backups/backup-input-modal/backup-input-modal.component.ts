import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-backup-input-modal',
  templateUrl: './backup-input-modal.component.html'
})
export class BackupInputModalComponent implements OnChanges {
  @Input() header: string = '';
  @Input() defaultInputText: string = '';
  @Input() error: string = '';
  @Input() visible: boolean = false;
  @Input() loading: boolean = false;

  @Output() onCancel: EventEmitter<any> = new EventEmitter();
  @Output() onConfirm: EventEmitter<string> = new EventEmitter<string>();

  inputText: string = '';

  get isVisible(): boolean {
    return this.visible;
  }
  set isVisible(value: boolean) {
    if (!value) {
      // support for 'x' / close button
      this.onCancel.emit();
    }
  }

  get hasError(): boolean {
    if(this.error)
      return true;
    return false;
  }

  ngOnChanges(changes: SimpleChanges): void {
    const visibilityChange = changes['visible'];
    if (visibilityChange && !visibilityChange.previousValue && visibilityChange.currentValue) {
      this.inputText = this.defaultInputText;
    }
  }

  confirm() {
    this.onConfirm.emit(this.inputText);
  }

  cancel() {
    this.onCancel.emit();
  }
}