import { Component, OnInit, Input, Output, ViewChild, EventEmitter } from '@angular/core';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { faTimes, faCat } from '@fortawesome/free-solid-svg-icons';

import { AuthenticationService } from '@app/_services/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['../home/home.component.css', './register.component.css']
})

export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  loading = false;
  faTimes = faTimes;
  mainIcon = faCat;

  @Output() registerClose = new EventEmitter<void>();

  @ViewChild(NgbPopover) public popover: NgbPopover;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      repeatedPassword: ['', Validators.required]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.registerForm.controls; }

  onSubmit() {
    if (this.registerForm.invalid) {
      this.popover.ngbPopover = "Type valid data";
      this.popover.popoverClass = "popover-error-reversed";
      this.popover.open();
      return;
    }

    if (this.f.password.value !== this.f.repeatedPassword.value) {
      this.popover.ngbPopover = "Passwords differ";
      this.popover.popoverClass = "popover-error-reversed";
      this.popover.open();
      return;
    }

    this.loading = true;
    this.authenticationService.register(this.f.username.value, this.f.password.value, this.f.repeatedPassword.value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.f.password.setValue("");
          this.f.repeatedPassword.setValue("");
          this.popover.ngbPopover = "Registered";
          this.popover.popoverClass = "popover-success-reversed";
          this.popover.open();
          this.loading = false;
        },
        error: error => {
          this.loading = false;
          this.popover.popoverClass = "popover-error-reversed";
          this.popover.ngbPopover = error === "Unexpected error" ? "Sign-Up Error" : error;
          this.popover.open();
        }
      });
  }
}
