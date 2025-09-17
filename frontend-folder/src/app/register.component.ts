import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DeviceService } from './device.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'register-component',
  standalone: true,         // ✅ mark standalone
  imports: [FormsModule,CommonModule],   // ✅ needed for ngModel
  template: `
    <h3>Register Device</h3>
    <label>
      Device ID:
      <input [(ngModel)]="deviceId" />
    </label>
    <label>
      Display Name:
      <input [(ngModel)]="displayName" />
    </label>
    <button (click)="register()">Register</button>

    <div *ngIf="msg" [ngClass]="{'success': success, 'error': !success}">
      {{ msg }}
    </div>
  `,
  styles: [`
    .success { color: green; margin-top: 8px; }
    .error { color: red; margin-top: 8px; }
  `],
  providers: [DeviceService]
})
export class RegisterComponent {
  deviceId = '';
  displayName = '';
  msg = '';
  success = false;

  constructor(private svc: DeviceService) {}

  register() {
    this.msg = '';
    this.success = false;

    this.svc.registerDevice(this.deviceId, this.displayName).subscribe({
      next: () => {
        this.msg = 'Device registered successfully';
        this.success = true;
      },
      error: e => {
        this.msg = 'Error: ' + (e?.message || e);
        this.success = false;
      }
    });
  }
}
