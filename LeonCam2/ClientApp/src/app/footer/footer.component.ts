import { Component, OnInit } from '@angular/core';
import { faGithub } from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {
  faGithub = faGithub;
  thisYear = new Date().getFullYear();

  constructor() { }

  ngOnInit(): void {
  }

}
