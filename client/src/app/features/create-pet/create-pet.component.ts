import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatOption, provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormField, MatHint, MatLabel } from '@angular/material/form-field';
import { MatSelect } from '@angular/material/select';
import { PetService } from '../../core/services/pet.service';
import { MatInputModule } from '@angular/material/input';
import { MatButton } from '@angular/material/button';
import { AccountService } from '../../core/services/account.service';
import { Router, RouterLink } from '@angular/router';


@Component({
  selector: 'app-create-pet',
  providers: [provideNativeDateAdapter()],
  imports: [
    MatFormField,
    MatLabel,
    MatHint,
    MatDatepickerModule,
    MatSelect,
    MatOption,
    ReactiveFormsModule,
    MatInputModule,
    MatButton,
    RouterLink
],
  templateUrl: './create-pet.component.html',
  styleUrl: './create-pet.component.scss'
})
export class CreatePetComponent {
  private fb = inject(FormBuilder);
  private petService = inject(PetService);
  private accountService = inject(AccountService);
  private router = inject(Router);

  currentUser = this.accountService.currentUser()?.id;

  petProfileForm = this.fb.group({
    id: ['', Validators.required],
    name: ['', Validators.required],
    breed: ['', Validators.required],
    species: ['', Validators.required],
    birthdate: ['', Validators.required],
    gender: ['', Validators.required],
    ownerId: [this.currentUser]
  });


  async createPet() {
    this.petService.createPetProfile(this.petProfileForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('/');
      }
    });
  }
}
