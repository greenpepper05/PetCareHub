import { Component, inject } from '@angular/core';
import { ServicesService } from '../../core/services/services.service';
import { RouterLink } from '@angular/router';
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-home',
  imports: [
    RouterLink,
    MatIcon
],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  title = 'PetChareHub'
  private servicesService = inject(ServicesService);
  services: any[] = [];

  ngOnInit(): void {
      this.servicesService.getServices().subscribe({
      next: response => this.services = response,
      error: error => console.log(error)
    })
  }
}
