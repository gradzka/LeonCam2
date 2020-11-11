import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-input-box',
  templateUrl: './input-box.component.html',
  styleUrls: ['./input-box.component.css'],
  viewProviders: [
    {
      provide: ControlContainer,
      useExisting: FormGroupDirective
    }
  ]
})
export class InputBoxComponent implements OnInit {
  @Input() autoComplete: string;
  @Input() class: string; 
  @Input() id: string;
  @Input() placeholder: string;
  @Input() type: string;

  constructor() { }

  ngOnInit(): void {
  }
}
