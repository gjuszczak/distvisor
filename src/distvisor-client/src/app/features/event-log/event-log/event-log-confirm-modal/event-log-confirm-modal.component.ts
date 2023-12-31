import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-event-log-confirm-modal',
  templateUrl: './event-log-confirm-modal.component.html'
})
export class EventLogConfirmModalComponent {
  @Input() header: string = '';
  @Input() operation: string = '';
  @Input() error: string = '';
  @Input() visible: boolean = false;
  @Input() loading: boolean = false;

  @Output() onCancel: EventEmitter<any> = new EventEmitter();
  @Output() onConfirm: EventEmitter<any> = new EventEmitter();

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

  confirm() {
    this.onConfirm.emit();
  }

  cancel() {
    this.onCancel.emit();
  }
}