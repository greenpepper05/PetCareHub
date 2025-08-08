import { Component, inject, OnInit } from '@angular/core';
import { ServicesService } from '../../../core/services/services.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-services-detail',
  imports: [],
  templateUrl: './services-detail.component.html',
  styleUrl: './services-detail.component.scss'
})
export class ServicesDetailComponent implements OnInit {
  private service = inject(ServicesService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  services: any[] = [];

  ngOnInit(): void {
    this.loadService()
  }

  loadService() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.service.getService(+id).subscribe({
      next: response => this.services = response
    })
  }
}
