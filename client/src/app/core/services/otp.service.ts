import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { RegisterFormValues } from '../../shared/models/registerFormValues';
import { RegisterWithOtpRequest } from '../../shared/models/registrationWithOtpRequest';
import { OtpSendRequest } from '../../shared/models/otpSendrequest';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class OtpService {
  private accountService = inject(AccountService);
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  private _isOtpSent = new BehaviorSubject<boolean>(false);
  isOtpSent$ = this._isOtpSent.asObservable();

  isOtpSent = this._isOtpSent.asObservable;

  public updateOtpService(isSent: boolean): void {
    this._isOtpSent.next(isSent);
  }

  public verifyOtpAndRegister(otp: string, fullRegistrationData: RegisterFormValues): Observable<any> {
    const finalPayload: RegisterWithOtpRequest = {
      ...fullRegistrationData,
      otp
    };

    return this.accountService.register(finalPayload).pipe(
      tap(() => this.updateOtpService(false))
    );

  }

  public sendOtp(payload: OtpSendRequest): Observable<any> {
    return this.accountService.sendOtp(payload).pipe(
      tap(() => this.updateOtpService(true))
    )
  }

  public validateOtp(email: string, otp: string) {
    return this.http.post<boolean>(`${this.baseUrl}otp/validate`, { email })
  }


}
