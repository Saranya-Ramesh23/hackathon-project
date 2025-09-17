import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterComponent } from './register.component';
import { RequestCertComponent } from './request-cert.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, RegisterComponent, RequestCertComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {}
