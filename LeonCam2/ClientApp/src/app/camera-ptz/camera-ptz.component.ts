import { Component, OnInit } from '@angular/core';
import { faCaretUp, faCaretLeft, faHome, faCaretRight, faCaretDown } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-camera-ptz',
  templateUrl: './camera-ptz.component.html',
  styleUrls: ['./camera-ptz.component.css']
})
export class CameraPTZComponent implements OnInit {
  faCaretUp = faCaretUp;
  faCaretLeft = faCaretLeft;
  faHome = faHome;
  faCaretRight = faCaretRight;
  faCaretDown = faCaretDown;

  constructor() { }

  ngOnInit(): void {
  }
}
