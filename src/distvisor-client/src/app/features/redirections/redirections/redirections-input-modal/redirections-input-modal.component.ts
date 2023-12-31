import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AbstractModalComponent } from 'src/app/shared/modal/abstract-modal.component';
import { Redirection, emptyRedirection } from '../../store/redirections.state';

@Component({
  selector: 'app-redirections-input-modal',
  templateUrl: './redirections-input-modal.component.html'
})
export class RedirectionsInputModalComponent extends AbstractModalComponent<Redirection> {
  @Input() value: Redirection = emptyRedirection;

  form = new FormGroup({
    id: new FormControl(''),
    name: new FormControl(''),
    url: new FormControl(''),
  });

  onShow() {
    this.form.setValue(this.value);
    this.autofocus();
  }

  protected getSubmitValue(): Redirection {
    let result = structuredClone(emptyRedirection);
    Object.assign(result, this.form.value);
    return result;
  }
}