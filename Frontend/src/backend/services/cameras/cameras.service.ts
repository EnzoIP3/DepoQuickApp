import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CamerasApiRepositoryService } from '../../repositories/cameras-api-repository.service';
import { CreateDeviceResponse } from '../devices/models/create-device-response';
import { CreateCameraRequest } from './models/create-camera-request';

@Injectable({
  providedIn: 'root'
})
export class CamerasService {

  constructor(private readonly _repository: CamerasApiRepositoryService) {}

    public postDevice(request: CreateCameraRequest): Observable<CreateDeviceResponse> {
        return this._repository.postDevice(request);
    }
}
