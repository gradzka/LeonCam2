import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './_services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html'
})
export class AppComponent {
  currentUser: string;
  sidebarWidth: number = 70;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['']);
  }
}
