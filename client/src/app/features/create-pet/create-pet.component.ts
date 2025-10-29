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
import { MatIcon } from '@angular/material/icon';
import { catchError, concatMap, finalize, flatMap, map, Observable, of } from 'rxjs';


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
    RouterLink,
    MatIcon
],
  templateUrl: './create-pet.component.html',
  styleUrl: './create-pet.component.scss'
})
export class CreatePetComponent {
  private fb = inject(FormBuilder);
  private petService = inject(PetService);
  private accountService = inject(AccountService);
  private router = inject(Router);

  selectedFile: File | null = null;
  imageUrl: string | ArrayBuffer | null = null;
  isUploading = false;

  currentUser = this.accountService.currentUser()?.id;

  petProfileForm = this.fb.group({
    name: ['', Validators.required],
    breed: ['', Validators.required],
    species: ['', Validators.required],
    birthdate: ['', Validators.required],
    gender: ['', Validators.required],
    ownerId: [this.currentUser],
    pictureUrl: ['']
  });


  createPet() {
    if (this.petProfileForm.invalid) {
            this.petProfileForm.markAllAsTouched();
            return;
        }

        this.isUploading = true;

        let upload: Observable<{ url: string }>;

        if (this.selectedFile) {
            const formData = new FormData();
            formData.append('File', this.selectedFile, this.selectedFile.name);

            upload = this.petService.uploadImage(formData).pipe(
                map(response => ({ url: response.url })),
                catchError(err => {
                    console.error('Image upload failed. Proceeding without picture.', err);
                    return of({ url: '' }); 
                })
            );
        } else {
            upload = of({ url: '' });
        }
        upload.pipe(
            concatMap(uploadResult => {
                const finalPayload = {
                    ...this.petProfileForm.value,
            
                    pictureUrl: uploadResult.url
                };

                return this.petService.createPetProfile(finalPayload);
            }),
            finalize(() => this.isUploading = false)
        ).subscribe({
            next: () => {
                console.log("Pet created successfully and navigated.");
                this.router.navigateByUrl('/pets'); 
            },
            error: (err) => {
                console.error('Pet creation failed:', err);
            }
        });
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];

    if (file) {
      this.selectedFile = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.imageUrl = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  uploadImage() : void {
    if (!this.selectedFile) {
      console.log("No Image");
      return;
    };

    this.isUploading = true;
    const formData = new FormData();

    formData.append('File', this.selectedFile, this.selectedFile.name);

    this.petService.uploadImage(formData)
      .pipe(
        finalize(() => this.isUploading = false)
      )
      .subscribe({
        next: (response) => {
          const pictureUrl = response.url;

          this.petProfileForm.patchValue({
            pictureUrl: pictureUrl
          })
        },
        error: (err) => {
          console.error('Image upload failed: ', err);

          this.selectedFile = null;
          this.imageUrl = null;
          this.petProfileForm.get('pictureUrl')?.setValue('');
        }
      })
  }
}
