import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastService } from './toasts/toast.service';

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
    private authenticationService: AuthenticationService,
    private toastService: ToastService
  ) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['']);
  }


  showStandard() {
    this.toastService.showStandard('I am a standard toast!!!!!!!!!!!!!!!!');
  }

  showSuccess() {
    this.toastService.showSuccess('I am a success toast');
  }

  showDanger() {
    this.toastService.showDanger("Danger");
  }

  showWarning() {
    this.toastService.showWarning("Warning");
  }

  showInfo() {
    this.toastService.showInfo("Info");
  }
}
