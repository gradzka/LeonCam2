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
}
