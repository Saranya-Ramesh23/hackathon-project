import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';

@Injectable({ providedIn: 'root' })
export class DeviceService {
  base = environment.apiBaseUrl;
  issue = environment.issueBaseUrl;

  constructor(private http: HttpClient) {}

  registerDevice(deviceId: string, displayName: string) {
    return this.http.post(`${this.base}/registry/devices`, {deviceId, displayName});
  }

  getDevice(deviceId: string) {
    return this.http.get(`${this.base}/registry/devices/${deviceId}`);
  }

  requestCertificate(deviceId: string) {
    return this.http.post(`${this.issue}/issuer/enroll`, { deviceId }, { responseType: 'text' as 'text' });
  }

  downloadCertificate(deviceId: string) {
    return this.http.get(`${this.issue}/issuer/certs/${deviceId}`, { responseType: 'blob' as 'blob' });
  }
}
