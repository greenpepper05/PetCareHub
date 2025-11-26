import { CommonModule } from '@angular/common';
import { Component, HostListener, inject, signal } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { Router, RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import { AccountService } from '../../../core/services/account.service';
import { MatButtonModule } from '@angular/material/button';
import {MatDividerModule} from '@angular/material/divider';
import { MatListModule} from '@angular/material/list';
import {MatIconModule} from '@angular/material/icon';
import { BusyService } from '../../../core/services/busy.service';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { User } from '../../../shared/models/user';
import { ClinicService } from '../../../core/services/clinic.service';
import { Clinic } from '../../../shared/models/clinic';
import { MatDialog } from '@angular/material/dialog';
import { AdminChangePasswordComponent } from '../../../shared/components/admin-change-password/admin-change-password.component';
@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [
    MatSidenavModule,
    RouterOutlet,
    RouterModule,
    CommonModule,
    MatButtonModule,
    MatListModule,
    MatDividerModule,
    MatIconModule,
    MatProgressBar,
    MatListModule,
    RouterLink,
    RouterLinkActive,
    MatToolbarModule,
    MatTooltipModule
  ],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  busyService = inject(BusyService);
  private clinicService = inject(ClinicService);
  private dialog = inject(MatDialog);
  clinic?: Clinic;
  admin?: User;

  sidebar: any;
  isMobile: boolean = false;
  isSidenavOpen: boolean = true;
  isSidebarCollapsed: boolean = false;
  isDropdownOpen = signal(false);

  private readonly mobileBreakpoint = 768; // Tailwind 'md' breakpoint
  readonly expandedWidth = 'w-64'; // 256px
  readonly collapsedWidth = 'w-16'; // 64px
  
  logout() {
    this.accountService.logout().subscribe({
      next: () => {
        this.accountService.currentUser.set(null);
        this.router.navigateByUrl('admin/login');
      }
    })
  }
  

  loadClinic() {
    
    this.clinicService.getClinicInfo().subscribe({
      next: (response) => {
        this.clinic = response;
        
      }
    })
  }

  loadUser() {
    this.accountService.getUserInfo().subscribe({
      next: (response) => {
        this.admin = response;
      }
    })
  }

  constructor() {
    this.checkScreenSize();
    this.loadClinic();
    this.loadUser();
  }

  @HostListener('window:resize', ['$event'])
    onResize($event: any) {
    this.checkScreenSize();
  }

  checkScreenSize() {
    const wasMobile = this.isMobile;
    this.isMobile = window.innerWidth < this.mobileBreakpoint;
    
    // Manage sidebar state based on device size
    if (this.isMobile) {
        this.isSidenavOpen = false; // Always closed initially on mobile, rely on toggle button
        this.isSidebarCollapsed = false; // No collapsing logic on mobile
    } else {
        // Desktop view: Use 'side' mode, and initial state is open
        this.isSidenavOpen = true;
    }
  }
  
  toggleMobileSidenav(sidenav: any) {
    sidenav.toggle();
  }
  
  // Desktop: Toggles the collapse state (width change)
  toggleSidebarCollapse() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  toggleDropdown(): void {
    this.isDropdownOpen.update(open => !open);
  }

  openChangePasswordModal() {
    const dialogRef = this.dialog.open(AdminChangePasswordComponent, {
      width: '400px'
    })
  }
}
