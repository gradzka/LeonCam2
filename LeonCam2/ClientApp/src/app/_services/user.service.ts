import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environments/environment';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private http: HttpClient) { }

  getAll() {
    //return this.http.get<string>(`${environment.apiUrl}/camera/GetCameras`);
    return this.http.get<string>(`/camera/GetCameras`);
  }
}
