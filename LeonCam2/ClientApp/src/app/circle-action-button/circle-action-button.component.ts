import { Component, OnInit, Input } from '@angular/core';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';

@Component({
  selector: 'app-circle-action-button',
  templateUrl: './circle-action-button.component.html',
  styleUrls: ['./circle-action-button.component.css']
})
export class CircleActionButtonComponent implements OnInit {

  @Input() icon: IconDefinition;
  @Input() class: string;
  @Input() callbackFunction: Function;

  constructor() { }

  ngOnInit(): void {
  }
}
