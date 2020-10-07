import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class UserService {
  private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

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
