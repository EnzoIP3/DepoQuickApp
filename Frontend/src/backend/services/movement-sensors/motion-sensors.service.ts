import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateDeviceResponse } from '../devices/models/create-device-response';
import { CreateMotionSensorRequest } from './models/create-motion-sensor-request';
import { MotionSensorsApiRepositoryService } from '../../repositories/motion-sensors-api-repository.service';

@Injectable({
  providedIn: 'root'
})
export class MotionSensorsService {

  constructor(private readonly _repository: MotionSensorsApiRepositoryService) {}

    public postDevice(request: CreateMotionSensorRequest): Observable<CreateDeviceResponse> {
        return this._repository.postDevice(request);
    }
}
