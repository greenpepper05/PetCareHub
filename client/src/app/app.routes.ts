import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ServicesComponent } from './features/services/services.component';
import { AppointmentComponent } from './features/appointment/appointment.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { authGuard } from './core/guards/auth-guard';
import { AppointmentSuccessComponent } from './features/appointment/appointment-success/appointment-success.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'services', component: ServicesComponent},
    {path: 'appointment', component: AppointmentComponent, canActivate: [authGuard]},
    {path: 'appointment/appointment-success', component: AppointmentSuccessComponent},
    {path: 'account/login', component: LoginComponent},
    {path: 'account/register', component: RegisterComponent},
    {path: '**', redirectTo: '', pathMatch: 'full'},
];
