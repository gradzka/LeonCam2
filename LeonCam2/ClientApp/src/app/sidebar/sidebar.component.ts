import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { faBars, faCat, faVideo, faAngleRight, faPlusCircle, faCogs, faSignOutAlt } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { AuthenticationService } from '@app/_services/authentication.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  faBars = faBars;
  faCat = faCat;
  faVideo = faVideo;
  faAngleRight = faAngleRight;
  faPlusCircle = faPlusCircle;
  faCogs = faCogs;
  faSignOutAlt = faSignOutAlt;

  @Input() sidebarWidth: number = 70;
  @Output() sidebarWidthChange = new EventEmitter<number>();

  constructor(private router: Router, private authenticationService: AuthenticationService) {
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/']);
    event.preventDefault();
  }

  toggleSidebar() {
    this.sidebarWidth = this.sidebarWidth == 70 ? 210 : 70;
    this.sidebarWidthChange.emit(this.sidebarWidth);
  }

  isPathActive(path: string) {
    return window.location.pathname.startsWith(path);
  }

  ngOnInit(): void {
    this.sidebarWidthChange.emit(this.sidebarWidth);
  }
}
