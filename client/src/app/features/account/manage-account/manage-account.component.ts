import { Component, inject } from '@angular/core';
import { FormsModule, NgForm, NgModel, ReactiveFormsModule } from '@angular/forms';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { AccountService } from '../../../core/services/account.service';
import { User } from '../../../shared/models/user';

interface UserProfile {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  contact: string;
  role: string;
  clinicId: number | null; // clinicId can be null if not associated
}

interface PasswordModel {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

@Component({
  selector: 'app-manage-account',
  standalone: true,
  imports: [
    MatIcon,
    MatLabel,
    MatFormField,
    FormsModule,
    ReactiveFormsModule,
    MatTabsModule
  ],
  templateUrl: './manage-account.component.html',
  styleUrl: './manage-account.component.scss'
})
export class ManageAccountComponent {
  private accountService = inject(AccountService);
  user?: User;

  userProfile: UserProfile = {
    id: '',
    firstName: '',
    lastName: '',
    email: '',
    contact: '',
    role: '',
    clinicId: null
  };

  // Initialize the password change model
  passwordModel: PasswordModel = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  // --- Constructor and Initialization ---
  constructor(
    // private accountService: AccountService, // Assume you inject this
    // private snackBar: MatSnackBar // For user feedback
  ) {}

  ngOnInit(): void {
    // 3. Load user data on initialization
    this.loadUserProfile();
  }

  // --- Methods ---

  private loadUserProfile(): void {
    // *****************************************************************
    // TODO: Replace this with actual data retrieval from your AccountService
    // *****************************************************************
    
    // Example of fetching and mapping data (replace with actual service call)
    const currentUserData = { 
      clinicId: 2, 
      contact: "09386089484", 
      email: "jimuelgaas@gmail.com", 
      firstName: "Jimuel", 
      id: "528d17f1-1a29-45d3-b3db-5408309155cd", 
      lastName: "Gaas", 
      role: "Admin" 
    };

    this.userProfile = { ...currentUserData }; // Use spread to safely copy data
    console.log('User profile loaded:', this.userProfile);
  }


  updateProfile(form: NgForm): void {
    if (form.valid) {
      // *****************************************************************
      // TODO: Implement API call to update the profile (firstName, lastName, contact)
      // *****************************************************************
      console.log('Attempting to update profile with:', this.userProfile);
      // Example success feedback:
      // this.snackBar.open('Profile updated successfully!', 'Close', { duration: 3000 });
    }
  }

  changePassword(form: NgForm): void {
    if (form.valid) {
      if (this.passwordModel.newPassword !== this.passwordModel.confirmPassword) {
        // Example error feedback:
        // this.snackBar.open('New passwords do not match.', 'Dismiss', { duration: 5000 });
        console.error('New passwords do not match.');
        return;
      }

      // *****************************************************************
      // TODO: Implement API call to change password using the model data
      // *****************************************************************
      console.log('Attempting to change password:', this.passwordModel);
      
      // Clear form on success:
      // form.resetForm();
      // this.snackBar.open('Password changed successfully!', 'Close', { duration: 3000 });
    }
  }
}
