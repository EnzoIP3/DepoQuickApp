import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CreateDeviceResponse } from '../services/devices/models/create-device-response';
import { CreateSensorRequest } from '../services/sensors/models/create-sensor-request';
import ApiRepository from './api-repository';

@Injectable({
  providedIn: 'root'
})
export class SensorsApiRepositoryService extends ApiRepository {
  constructor(http: HttpClient) {
      super(environment.apiUrl, "sensors", http);
  }

  public postDevice(
      request: CreateSensorRequest
  ): Observable<CreateDeviceResponse> {
      return this.post<CreateDeviceResponse>(request);
  }
}
