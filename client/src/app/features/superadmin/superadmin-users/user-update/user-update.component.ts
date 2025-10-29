import { Component, inject, OnInit, signal } from '@angular/core';
import { AccountService } from '../../../../core/services/account.service';
import { User } from '../../../../shared/models/user';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-user-update',
  imports: [
    NgClass,
    RouterLink,
    MatIcon
  ],
  templateUrl: './user-update.component.html',
  styleUrl: './user-update.component.scss'
})
export class UserUpdateComponent implements OnInit{
  private accountService = inject(AccountService);
  private activatedRoute = inject(ActivatedRoute);
  user?: User;

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if (!id) return;

    this.accountService.getUserWithClinic(id).subscribe({
      next: response => {
        this.user = response
      }
    });
  }

  onDelete() {

  }

  copyStatus = signal<'idle' | 'copied'>('idle');

    /**
     * Copies the given text to the clipboard using document.execCommand('copy').
     * @param text The string to copy.
     */
    copyToClipboard(text: string): void {
        const tempElement = document.createElement('textarea');
        tempElement.value = text;
        document.body.appendChild(tempElement);
        tempElement.select();

        try {
            document.execCommand('copy');
            this.copyStatus.set('copied');
            
            // Reset status after 2 seconds
            setTimeout(() => this.copyStatus.set('idle'), 2000);
        } catch (err) {
            console.error('Could not copy text to clipboard', err);
        } finally {
            document.body.removeChild(tempElement);
        }
    }
}
