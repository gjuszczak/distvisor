import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthorizeService } from '../authorize.service';
import { ApplicationPaths, ReturnUrlType } from '../authorization.constants';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html'
})
export class LogoutComponent implements OnInit {
  public message = new BehaviorSubject<string>(null);

  constructor(
    private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  async ngOnInit() {
    this.authorizeService.signOut();
    this.router.navigate(ApplicationPaths.LoginPathComponents);
  }
}