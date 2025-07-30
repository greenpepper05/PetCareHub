import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ServicesComponent } from './features/services/services.component';
import { AppointmentComponent } from './features/appointment/appointment.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'services', component: ServicesComponent},
    {path: 'appointment', component: AppointmentComponent},
    {path: '**', redirectTo: '', pathMatch: 'full'},
];
