import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { JwtInterceptor } from './_helpers/jwt.interceptor';
import { ErrorInterceptor } from './_helpers/error.interceptor';
import { LoginComponent } from './login/login.component';
import { LoggedGuard } from './_helpers/logged.guard';
import { GuestGuard } from './_helpers/guest.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { InputBoxComponent } from './input-box/input-box.component';
import { PasswordBoxComponent } from './password-box/password-box.component';
import { RegisterComponent } from './register/register.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FooterComponent } from './footer/footer.component';
import { NgsgModule } from 'ng-sortgrid';
import { CommonModule } from "@angular/common";
import { CameraBoxComponent } from './camera-box/camera-box.component';
import { CircleActionButtonComponent } from './circle-action-button/circle-action-button.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [GuestGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [LoggedGuard] },

  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    HomeComponent,
    InputBoxComponent,
    PasswordBoxComponent,
    RegisterComponent,
    SidebarComponent,
    FooterComponent,
    CameraBoxComponent,
    CircleActionButtonComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes),
    FontAwesomeModule,
    NgbModule,
    BrowserAnimationsModule,
    NgsgModule,
    CommonModule],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
