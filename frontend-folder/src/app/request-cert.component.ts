import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DeviceService } from './device.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'request-cert-component',
  standalone: true,
  imports: [FormsModule,CommonModule],
  template: `
    <h3>Request Certificate</h3>
    <label>
      Device ID:
      <input [(ngModel)]="deviceId" />
    </label>
    <button (click)="request()">Request Certificate</button>
    <button (click)="download()" [disabled]="!deviceId">Download Cert</button>

    <div *ngIf="successMsg" class="success">{{ successMsg }}</div>
    <div *ngIf="errorMsg" class="error">{{ errorMsg }}</div>
  `,
  styles: [`
    .success { color: green; margin-top: 8px; }
    .error { color: red; margin-top: 8px; }
  `]
})

export class RequestCertComponent {
  deviceId = '';
  successMsg = '';
  errorMsg = '';

  constructor(private svc: DeviceService) {}

  request() {
    this.clearMsgs();
    this.svc.requestCertificate(this.deviceId).subscribe({
      next: r => this.successMsg = 'Enrollment requested. ',
      error: e => this.errorMsg = 'Error: ' + (e?.message.innerText || e)
    });
  }

  download() {
    this.clearMsgs();
    this.svc.downloadCertificate(this.deviceId).subscribe({
      next: blob => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${this.deviceId}.pfx`;
        a.click();
        window.URL.revokeObjectURL(url);
        this.successMsg = 'Certificate downloaded';
      },
      error: e => this.errorMsg = 'Error downloading: ' + (e?.message || e)
    });
  }

  private clearMsgs() {
    this.successMsg = '';
    this.errorMsg = '';
  }
}
