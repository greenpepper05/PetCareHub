import { Component, inject, OnInit } from '@angular/core';
import { ServicesService } from '../../core/services/services.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-services',
  imports: [
    RouterLink
  ],
  templateUrl: './services.component.html',
  styleUrl: './services.component.scss'
})
export class ServicesComponent implements OnInit {
  private servicesService = inject(ServicesService);
  services: any[] = [];
  
  ngOnInit(): void {
      this.servicesService.getServices().subscribe({
      next: response => this.services = response,
      error: error => console.log(error)
    })
  }

}
