import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-password-box',
  templateUrl: './password-box.component.html',
  styleUrls: ['../input-box/input-box.component.css', './password-box.component.css'],
  viewProviders: [
    {
      provide: ControlContainer,
      useExisting: FormGroupDirective
    }
  ]
})

export class PasswordBoxComponent implements OnInit {

  eyeClicked: boolean = false;
  ico = faEye;
  @Input() id: string;
  @Input() placeholder: string;
  @Input() value: string;
  @Output() valueChange = new EventEmitter<string>();
  @Input() autoComplete: string;
  @Input() withPasswordStrength: boolean;

  constructor() { }

  ngOnInit(): void {
  }

  checkPasswordStrength(event: any) {
    const hasNumber = value => {
      return new RegExp(/[0-9]/).test(value);
    };
    const hasMixed = value => {
      return new RegExp(/[a-z]/).test(value) && new RegExp(/[A-Z]/).test(value);
    };
    const hasSpecial = value => {
      return new RegExp(/[!#@$%^&*)(+=._-]/).test(value);
    }

    let bar = event.target.parentNode.getElementsByClassName('password-strength-bar')[1];
    bar.classList.remove(bar.classList[1]);

    let passwordValue = event.target.value;
    if (passwordValue.length > 0) {
      let strengths = 0;
      if (passwordValue.length > 5) {
        strengths++;
      }
      if (passwordValue.length > 7) {
        strengths++;
      }
      if (hasNumber(passwordValue)) {
        strengths++;
      }
      if (hasSpecial(passwordValue)) {
        strengths++;
      }
      if (hasMixed(passwordValue)) {
        strengths++;
      }

      if (strengths === 5) {
        bar.classList.add('very-strong');
      }
      else if (strengths === 4) {
        bar.classList.add('strong');
      }
      else if (strengths === 3) {
        bar.classList.add('normal');
      }
      else if (strengths === 2) {
        bar.classList.add('weak');
      }
      else {
        bar.classList.add('very-weak');
      }
    }
  }

  eyeChanged(event: any) {
    this.eyeClicked = event.type === "mousedown" || event.type === "touchstart";
    this.ico = this.eyeClicked ? faEye : faEyeSlash;
  }

  hideCapsLkWarning(event: any) {
    let inputContainer = event.target.parentNode;
    inputContainer.childNodes[1].childNodes[1].style.visibility = "hidden";
  }

  toggleCapsLkWarning(event: any) {
    let inputContainer = event.target.parentNode;

    if (event.getModifierState('CapsLock')) {
      inputContainer.childNodes[1].childNodes[1].style.visibility = "visible";
    }
    else {
      inputContainer.childNodes[1].childNodes[1].style.visibility = "hidden";
    }
  }
}
