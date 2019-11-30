import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizeService } from '../authorize.service';
import { ApplicationPaths } from '../authorization.constants';
import { UserService } from '../user.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html'
})
export class LogoutComponent implements OnInit {
  constructor(
    private authorizeService: AuthorizeService,
    private userService: UserService,
    private router: Router) { }

  ngOnInit() {
    this.authorizeService.logout()
      .pipe(finalize(() => {
          this.userService.clearUser();
          this.router.navigate(ApplicationPaths.LoginPathComponents);
      }))
      .subscribe();
  }
}
