import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Camera } from '../shared/models/camera.model';

@Injectable({ providedIn: 'root' })
export class CameraService {
  private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  addCamera(camera: Camera) {
    return this.http.post<any>(`${this.baseUrl}cameras/addCamera`, JSON.stringify(camera), this.httpOptions)
      .pipe(map(data => { return data; }));
  }

  editCamera(camera: Camera) {
    return this.http.post<any>(`${this.baseUrl}cameras/editCamera`, JSON.stringify(camera), this.httpOptions)
      .pipe(map(data => { return data; }));
  }

  get(id: number) {
    return this.http.get<any>(`${this.baseUrl}cameras/get/${id}`, this.httpOptions)
      .pipe(map(data => { return data; }));
  }

  getUserCameras() {
    return this.http.get<Camera[]>(`${this.baseUrl}cameras/GetUserCameras`, this.httpOptions)
      .pipe(map(data => { return data; }));
  }

  discover() {
    return this.http.get<any>(`${this.baseUrl}cameras/discover`, this.httpOptions)
      .pipe(map(data => { return data; }));
  }
}
