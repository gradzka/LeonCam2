import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class UserService {
  private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  changeUsername(newUsername: string, password: string) {
    return this.http.post<any>(`${this.baseUrl}users/changeUsername`, { newUsername, password })
      .pipe(map(data => { return data; }));
  }

  changePassword(oldPassword: string, newPassword: string, confirmNewPassword: string) {
    return this.http.post<any>(`${this.baseUrl}users/changePassword`, { oldPassword, newPassword, confirmNewPassword })
      .pipe(map(data => { return data; }));
  }

  resetAccount(password: string) {
    return this.http.post<any>(`${this.baseUrl}users/resetAccount`, { password })
      .pipe(map(data => { return data; }));
  }

  deleteAccount(password: string) {
    return this.http.post<any>(`${this.baseUrl}users/deleteAccount`, { password })
      .pipe(map(data => { return data; }));
  };

  getAll() {
    return this.http.get<string[]>(`${this.baseUrl}camera/getcameras`);
  }

  getOne() {
    return this.http.get<any>(`${this.baseUrl}camera/getuser`);
  }

  getOneStr() {
    return this.http.get(`${this.baseUrl}camera/getcamera`, { responseType: 'text' });
  }
}
