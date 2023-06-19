import { Component, Input } from '@angular/core';
import { AbstractModalComponent } from 'src/app/shared/modal/abstract-modal.component';
import { Redirection, emptyRedirection } from '../../store/redirections.state';

@Component({
  selector: 'app-redirections-confirm-modal',
  templateUrl: './redirections-confirm-modal.component.html'
})
export class RedirectionsConfirmModalComponent extends AbstractModalComponent<Redirection> {  
  @Input() value: Redirection = emptyRedirection;

  protected getSubmitValue(): Redirection {
    return this.value;
  }
}