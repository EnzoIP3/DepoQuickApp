import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TableComponent } from '../../components/table/table.component';

interface BusinessDevice {
  id: string;
  name: string;
  modelNumber: string;
  description: string;
  mainPhoto: string;
  secondaryPhotos: string[];
  type: string;
}

@Component({
  selector: 'app-business-devices-table',
  standalone: true,
  imports: [CommonModule, TableComponent],
  templateUrl: './business-devices-table.component.html',
})

export class BusinessDevicesTableComponent {
  columns = [
    { field: 'id', header: 'Device Id' },
    { field: 'name', header: 'Device Name' },
    { field: 'modelNumber', header: 'Model Number' },
    { field: 'description', header: 'Description' },
    { field: 'mainPhoto', header: 'Main Photo' },
    { field: 'secondaryPhotos', header: 'Secondary Photos' },
    { field: 'type', header: 'Device Type' }
  ];

  // Datos de ejemplo (dummy)
  devices: BusinessDevice[] = [
    {
      id: '1',
      name: 'Device 1',
      modelNumber: 'D1-123',
      description: 'Description of Device 1',
      mainPhoto: 'https://example.com/photo1.jpg',
      secondaryPhotos: ['https://example.com/photo2.jpg', 'https://example.com/photo3.jpg'],
      type: 'Camera'
    },
    {
      id: '2',
      name: 'Device 2',
      modelNumber: 'D2-456',
      description: 'Description of Device 2',
      mainPhoto: 'https://example.com/photo4.jpg',
      secondaryPhotos: ['https://example.com/photo5.jpg', 'https://example.com/photo6.jpg'],
      type: 'Sensor'
    },
    {
      id: '3',
      name: 'Device 3',
      modelNumber: 'D3-789',
      description: 'Description of Device 3',
      mainPhoto: 'https://example.com/photo7.jpg',
      secondaryPhotos: ['https://example.com/photo8.jpg', 'https://example.com/photo9.jpg'],
      type: 'Thermostat'
    }
  ];

  loading: boolean = false;  // Aquí podrías usar este estado si necesitas cargar datos

  constructor(private readonly _router: Router) {}

  onRowClick(device: BusinessDevice) {
    this._router.navigate(['/devices', device.id]);  // Ajusta la ruta según tu estructura
  }
}
