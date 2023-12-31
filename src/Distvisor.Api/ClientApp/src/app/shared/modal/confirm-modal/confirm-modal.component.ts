import { Component, ElementRef, EventEmitter, Input, Output, QueryList, ViewChildren } from '@angular/core';

@Component({
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.component.html'
})
export class ConfirmModalComponent {
  @Input() header: string = '';
  @Input() error: string = '';
  @Input() visible: boolean = false;
  @Input() loading: boolean = false;
  @Input() submitBtn: string = 'Yes';
  @Input() cancelBtn: string = 'No';

  @Output() onSubmit: EventEmitter<any> = new EventEmitter();
  @Output() onCancel: EventEmitter<any> = new EventEmitter();

  @ViewChildren('autofocus') autofocusElements!: QueryList<ElementRef>;

  get isVisible(): boolean {
    return this.visible;
  }
  set isVisible(value: boolean) {
    if (!value) {
      this.cancel(); // support for close button -> 'x'
    }
  }
  
  autofocus() {
    if (this.autofocusElements && this.autofocusElements.length > 0) {
        this.autofocusElements.first.nativeElement.focus();
    }
  }    

  submit() {
    this.onSubmit.emit();
  }

  cancel() {
    this.onCancel.emit();
  }
}