import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-input-modal',
  templateUrl: './input-modal.component.html'
})
export class InputModalComponent {
  @Input() header: string = '';
  @Input() visible: boolean = false;
  @Input() loading: boolean = false;

  @Output() onClose: EventEmitter<any> = new EventEmitter();
  @Output() onShow: EventEmitter<any> = new EventEmitter();

  get isVisible(): boolean {
    return this.visible;
  }
  set isVisible(value: boolean) {
    if (!value) {
      this.onClose.emit(); // support for close button -> 'x'
    }
  }

  show() {
    this.onShow.emit();
  }
}