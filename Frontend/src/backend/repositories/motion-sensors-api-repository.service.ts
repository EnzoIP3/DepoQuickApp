import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CreateDeviceResponse } from '../services/devices/models/create-device-response';
import ApiRepository from './api-repository';
import { CreateMotionSensorRequest } from '../services/movement-sensors/models/create-motion-sensor-request';

@Injectable({
  providedIn: 'root'
})
export class MotionSensorsApiRepositoryService extends ApiRepository {
  constructor(http: HttpClient) {
      super(environment.apiUrl, "motion_sensors", http);
  }

  public postDevice(
      request: CreateMotionSensorRequest
  ): Observable<CreateDeviceResponse> {
      return this.post<CreateDeviceResponse>(request);
  }
}

