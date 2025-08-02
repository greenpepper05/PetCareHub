import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { AccountService } from '../../../core/services/account.service';
import { MatButton } from '@angular/material/button';
import {MatDividerModule} from '@angular/material/divider';
import {MatListModule} from '@angular/material/list';
import {MatIconModule} from '@angular/material/icon';
@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [
    MatSidenavModule,
    RouterOutlet,
    RouterModule,
    CommonModule,
    MatButton,
    MatListModule,
    MatDividerModule,
    MatIconModule
  ],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  accountService = inject(AccountService);
  private router = inject(Router);

  logout() {
    this.accountService.logout().subscribe({
      next: () => {
        this.accountService.currentUser.set(null);
        this.router.navigateByUrl('admin/login');
      }
    })
  }
}
