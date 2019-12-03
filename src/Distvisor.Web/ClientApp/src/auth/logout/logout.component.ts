import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { ApplicationPaths } from '../auth.constants';
import { UserService } from '../user.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html'
})
export class LogoutComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router) { }

  ngOnInit() {
    this.authService.logout()
      .pipe(finalize(() => {
          this.userService.clearUser();
          this.router.navigate(ApplicationPaths.LoginPathComponents);
      }))
      .subscribe();
  }
}
