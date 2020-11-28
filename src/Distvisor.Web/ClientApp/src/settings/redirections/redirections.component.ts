import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { RedirectionsService } from '../../api/services';
import { RedirectionDetails } from '../../api/models';

@Component({
  selector: 'app-redirections',
  templateUrl: './redirections.component.html'
})
export class RedirectionsComponent implements OnInit, OnDestroy {

  private subscriptions: Subscription[] = [];
  storedRedirections: RedirectionDetails[] = [];
  inputRedirectionName: string = '';
  inputRedirectionUrl: string = '';

  constructor(private redirectionsService: RedirectionsService) { }

  ngOnInit() {
    this.reloadList();
  }

  reloadList() {
    this.subscriptions.push(this.redirectionsService.apiRedirectionsGet$Json()
      .subscribe(redirections => {
        this.storedRedirections = redirections;
      }));
  }

  onSave() {
    this.subscriptions.push(this.redirectionsService.apiRedirectionsPost({
      body:{
        name: this.inputRedirectionName,
        url: this.inputRedirectionUrl
      }
    }).subscribe(() => this.reloadList()));
  }

  onRemove(name: string) {
    this.subscriptions.push(this.redirectionsService.apiRedirectionsNameDelete({
      name: name,
    }).subscribe(() => this.reloadList()));
  }

  isLast(redirection: RedirectionDetails) {
    return this.storedRedirections.indexOf(redirection) === this.storedRedirections.length -1;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
