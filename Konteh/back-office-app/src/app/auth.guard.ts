import { CanActivateFn, Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { inject } from '@angular/core';
import { Observable, of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const msalService = inject(MsalService);
  const router = inject(Router);

  const accounts = msalService.instance.getAllAccounts();

  if (accounts.length > 0) {
    return true;
  } else {
    return false;
  }
};