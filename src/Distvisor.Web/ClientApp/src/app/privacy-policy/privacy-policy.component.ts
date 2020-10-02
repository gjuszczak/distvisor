import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-privacy-policy',
  templateUrl: './privacy-policy.component.html'
})
export class PrivacyPolicyComponent {

  websiteUrl: string

  constructor(@Inject('BASE_URL') baseUrl: string) {
    this.websiteUrl = baseUrl;
  }
}