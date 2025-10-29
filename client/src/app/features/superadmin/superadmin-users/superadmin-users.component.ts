import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../core/services/account.service';
import { Validators } from '@angular/forms';
import { User } from '../../../shared/models/user';
import { MatDialog } from '@angular/material/dialog';
import { AddNewUserComponent } from '../../../shared/components/add-new-user/add-new-user.component';
import { MatIcon } from '@angular/material/icon';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-superadmin-users',
  imports: [
    MatIcon,
    RouterLink
],
  templateUrl: './superadmin-users.component.html',
  styleUrl: './superadmin-users.component.scss'
})
export class SuperadminUsersComponent implements OnInit {
  private accountService = inject(AccountService);
  private dialog = inject(MatDialog);
  users: User[] = [];

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.accountService.getAllAdminUsers().subscribe({
      next: (data) => {
        this.users = data;
      }
    })
  }

  openUserRegistrationModal() {
    const dialogRef = this.dialog.open(AddNewUserComponent, {
      width: '450px',
      panelClass: 'custom-dialog-container',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadUsers();
      }
    });
  }
}
