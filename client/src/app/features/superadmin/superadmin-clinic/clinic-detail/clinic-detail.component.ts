import { Component, inject, OnInit } from '@angular/core';
import { ClinicService } from '../../../../core/services/clinic.service';
import { Clinic } from '../../../../shared/models/clinic';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AccountService } from '../../../../core/services/account.service';
import { User } from '../../../../shared/models/user';
import { MatIcon } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { SnackbarService } from '../../../../core/services/snackbar.service';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-clinic-detail',
  imports: [
    MatIcon,
    RouterLink,
    DatePipe
],
  templateUrl: './clinic-detail.component.html',
  styleUrl: './clinic-detail.component.scss'
})
export class ClinicDetailComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private clinicService = inject(ClinicService);
  private accountService = inject(AccountService);
  private router = inject(Router);
  private snackbarService = inject(SnackbarService);
  clinic?: Clinic;
  user?: User;

  ngOnInit(): void {
    this.loadClinic();
  }

  loadClinic() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if (!id) return;

    this.clinicService.getClinicById(+id).subscribe({
      next: response => {
        this.clinic = response
        this.loadUser(response.ownerId);
        
      }
    });
  }

  loadUser(id: string) {
    this.accountService.getUserById(id).subscribe({
          next: data => {
            this.user = data
          }
    })
  }

  onDelete() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if (!id) return;

    this.clinicService.deleteClinic(+id).subscribe({
      next: () => {
        this.router.navigate(['superadmin/clinics']);
        this.snackbarService.success("Clinic Deleted!");
      },
      error: (err) => {
        this.snackbarService.error("An error occured: " + err);
      }
    });
  }
}
