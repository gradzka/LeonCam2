import { Component } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [trigger('openCloseAnimation', [transition(
    ':enter',
    [
      style({ height: 0, opacity: 0 }),
      animate('0.5s ease-out', style({ opacity: 1}))
    ]
  ),
    //transition(
    //  ':leave',
    //  [
    //    style({ height: 300, opacity: 1 }),
    //    animate('0.5s ease-in', style({ opacity: 0 }))
    //  ]
    //)
  ])]
})
export class HomeComponent {
  isRegistration: boolean = false;
}
