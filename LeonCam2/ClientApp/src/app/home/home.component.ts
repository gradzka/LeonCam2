import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '@app/_services/authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  constructor(router: Router, authenticationService: AuthenticationService) {
    // redirect to home if already logged in
    if (authenticationService.currentUserValue) {
      router.navigate(['/dashboard']);
    }
  }
}
