import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { inject } from '@angular/core';
import { map, of } from 'rxjs';

export const superadminAuthGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  
  if (accountService.currentUser()?.role === 'SuperAdmin') {
      return of(true);
  }

  return accountService.getAuthState().pipe(
      map(auth => {
        const currentUser = accountService.currentUser();
        if (auth.isAuthenticated && currentUser?.role === 'SuperAdmin') {
          router.navigate(['superadmin/dashboard'])
          return true;
        } else {
          router.navigate(['superadmin/login'], { queryParams: { returnUrl: state.url}
          });
          return false;
        }
      })
  )
};
