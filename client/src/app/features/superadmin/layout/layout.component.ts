import { NgClass } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterModule } from "@angular/router";
import { AccountService } from '../../../core/services/account.service';
import { BusyService } from '../../../core/services/busy.service';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-layout',
  imports: [
    NgClass,
    RouterModule,
    MatProgressBar,
    RouterLink,
    MatIcon
],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  busyService = inject(BusyService);

  // Data for the SuperAdmin (Updated)
  readonly superAdmin = {
    firstName: 'Petcarehub',
    lastName: '', 
    role: 'Administrator',
  };
  
  readonly systemName = 'Super Admin Portal';
  // Image Placeholder (40x40, Dark Teal with 'SA' text)
  readonly pictureUrl = 'https://placehold.co/40x40/008080/ffffff?text=SA'; 

  // Navigation Links (Updated routes to reflect platform/SA level)
  readonly navigationLinks = [
    { title: 'Platform Dashboard', route: '/superadmin/dashboard', icon: 'monitor' },
    { title: 'Manage Clinics', route: '/superadmin/clinics', icon: 'business' },
    { title: 'User Management', route: '/superadmin/users', icon: 'group' },
    // { title: 'System Settings', route: '/superadmin/settings', icon: 'settings' },
  ];

  // --- LAYOUT STATE ---
  isMobile = signal(false);
  isSidebarCollapsed = signal(false);
  isSidenavOpen = signal(false);
  isDropdownOpen = signal(false);

  // Width Classes
  readonly expandedWidth = 'w-64';
  readonly collapsedWidth = 'w-20';

  // Computed display name
  readonly adminDisplayName = computed(() => 
    `${this.superAdmin.firstName} ${this.superAdmin.lastName}`.trim()
  );

  constructor() {
    // Check for window to ensure SSR safety and setup resize listener
    if (typeof window !== 'undefined') {
      this.checkIsMobile();
      window.addEventListener('resize', () => this.checkIsMobile());
    }
  }

  // Determine if the screen is mobile size
  private checkIsMobile(): void {
    const isCurrentlyMobile = window.innerWidth < 768;
    this.isMobile.set(isCurrentlyMobile);
    // If we transition to desktop, ensure the mobile overlay is closed
    if (!isCurrentlyMobile && this.isSidenavOpen()) {
        this.isSidenavOpen.set(false);
    }
  }

  // Toggles the sidebar state (used by the menu button)
  toggleSidebarCollapse() {
    if (this.isMobile()) {
      // On mobile, toggle the overlay sidenav
      this.isSidenavOpen.update(val => !val);
    } else {
      // On desktop, toggle the collapse/expand state
      this.isSidebarCollapsed.update(val => !val);
    }
  }

  // Specifically for closing the mobile sidenav
  toggleMobileSidenav() {
    this.isSidenavOpen.set(false);
  }

  logout() {
    this.accountService.logout().subscribe({
      next: () => {
        this.accountService.currentUser.set(null);
        this.router.navigateByUrl('admin/login');
      }
    })
  }

  toggleDropdown(): void {
    this.isDropdownOpen.update(open => !open);
  }
}
