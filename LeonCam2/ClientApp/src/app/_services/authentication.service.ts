import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<string>;
  public currentUser: Observable<string>;
  private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.currentUserSubject = new BehaviorSubject<string>(localStorage.getItem('currentUser'));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): string {
    return this.currentUserSubject.value;
  }

  getLeadingQuestion(username: string) {
    return this.http.post<any>(`${this.baseUrl}users/GetLeadingQuestion`, { username })
      .pipe(map(data => { return data; }));
  }

  login(username: string, password: string) {
    return this.http.post<any>(`${this.baseUrl}users/login`, { username, password })
      .pipe(map(user => { return this.setCurrentUser(user.token); }));
}

  register(username: string, password: string, repeatedPassword: string) {
    return this.http.post<any>(`${this.baseUrl}users/register`, { username, password, repeatedPassword })
      .pipe(map(data => { return data; }));
  }

  checkAnswer(username: string, answer: string) {
    return this.http.post<any>(`${this.baseUrl}users/checkanswer`, { username, answer })
      .pipe(map(user => { return this.setCurrentUser(user.token); }));
  }

  logout() {
    this.http.post<any>(`${this.baseUrl}users/logout`, {});
    this.setCurrentUser(null);
  }

  setCurrentUser(token: string) {
    if (token == null) {
      localStorage.removeItem('currentUser');
    }
    else {
      localStorage.setItem('currentUser', token);
    }

    this.currentUserSubject.next(token);
    return token;
  }
}
