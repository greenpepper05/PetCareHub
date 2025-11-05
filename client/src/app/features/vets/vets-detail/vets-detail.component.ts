import { Component, inject, OnInit } from '@angular/core';
import { ClinicService } from '../../../core/services/clinic.service';
import { Clinic } from '../../../shared/models/clinic';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { CurrencyPipe } from '@angular/common';
import { ServicesService } from '../../../core/services/services.service';
import { Services } from '../../../shared/models/services';

@Component({
  selector: 'app-vets-detail',
  imports: [
    MatIcon,
    CurrencyPipe,
    RouterLink
],
  templateUrl: './vets-detail.component.html',
  styleUrl: './vets-detail.component.scss'
})
export class VetsDetailComponent implements OnInit{
  private activatedRoute = inject(ActivatedRoute);
  private clinicService = inject(ClinicService);
  private serviceService = inject(ServicesService);
  services: Services[] = [];
  clinic?: Clinic;

  ngOnInit(): void {
    this.loadClinic();
  }

  async loadClinic() {
    const id = this.activatedRoute.snapshot.paramMap.get("id");

    if (!id) return;

    this.clinicService.getClinicDetail(+id).subscribe({
      next: data => {
        this.clinic = data
        this.LoadServices(data.id);
      }
    })
  }

  async LoadServices(id: number){
    this.serviceService.getServiceByClinic(id).subscribe({
      next: data => {
        this.services = data;
      }
    })
  }

}
