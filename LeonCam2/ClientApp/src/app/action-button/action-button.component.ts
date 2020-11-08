import { Component, OnInit, Input } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';

@Component({
  selector: 'app-action-button',
  templateUrl: './action-button.component.html',
  styleUrls: ['./action-button.component.css', '../circle-action-button/circle-action-button.component.css']
})
export class ActionButtonComponent implements OnInit {

  @Input() icon: IconDefinition;
  @Input() class: string;
  @Input() callbackFunction: Function;
  @Input() size: string;

  constructor() { }

  ngOnInit(): void {
  }

}
