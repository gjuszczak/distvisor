import { Component, ElementRef, EventEmitter, Input, Output, QueryList, ViewChildren } from '@angular/core';

@Component({ template: '' })
export abstract class AbstractModalComponent<T> {
    @Input() visible: boolean = false;
    @Input() operation: string = '';
    @Input() error: string = '';
    @Input() loading: boolean = false;

    @Output() onSubmit: EventEmitter<T> = new EventEmitter<T>();
    @Output() onCancel: EventEmitter<any> = new EventEmitter();

    @ViewChildren('autofocus') autofocusElements!: QueryList<ElementRef>;

    protected abstract getSubmitValue(): T;

    autofocus() {
        if (this.autofocusElements && this.autofocusElements.length > 0) {
            this.autofocusElements.first.nativeElement.focus();
        }
    }    

    submit() {
        this.onSubmit.emit(this.getSubmitValue());
    }

    cancel() {
        this.onCancel.emit();
    }
}