import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SensorsApiRepositoryService } from '../../repositories/sensors-api-repository.service';
import { CreateDeviceResponse } from '../devices/models/create-device-response';
import { CreateSensorRequest } from './models/create-sensor-request';

@Injectable({
  providedIn: 'root'
})
export class SensorsService {

  constructor(private readonly _repository: SensorsApiRepositoryService) {}

    public postDevice(request: CreateSensorRequest): Observable<CreateDeviceResponse> {
        return this._repository.postDevice(request);
    }
}
