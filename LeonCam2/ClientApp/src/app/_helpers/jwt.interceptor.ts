import { Inject, Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthenticationService } from '@app/_services/authentication.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private baseUrl: string;

  constructor(private authenticationService: AuthenticationService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const currentUser = this.authenticationService.currentUserValue;
    const isApiUrl = request.url.startsWith(this.baseUrl);
    if (currentUser && isApiUrl) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser}`
        }
      });
    }

    return next.handle(request);
  }
}
