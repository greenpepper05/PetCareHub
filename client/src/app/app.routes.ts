import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ServicesComponent } from './features/services/services.component';
import { AppointmentComponent } from './features/appointment/appointment.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { authGuard } from './core/guards/auth-guard';
import { AppointmentSuccessComponent } from './features/appointment/appointment-success/appointment-success.component';
import { MyappointmentsComponent } from './features/myappointments/myappointments.component';
import { MyappointmentsDetailComponent } from './features/myappointments/myappointments-detail/myappointments-detail.component';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { DashboardComponent } from './features/admin/dashboard/dashboard.component';
import { AdminLayoutComponent } from './features/admin/admin-layout/admin-layout.component';
import { AppointmentsComponent } from './features/admin/appointments/appointments.component';
import { PetsComponent } from './features/admin/pets/pets.component';
import { AdminLoginComponent } from './features/admin/admin-account/admin-login/admin-login.component';
import { adminAuthGuard } from './core/guards/admin-auth-guard';
import { MyPetsComponent } from './features/my-pets/my-pets.component';
import { CreatePetComponent } from './features/create-pet/create-pet.component';

export const routes: Routes = [
    {
        path: 'admin',
        component: AdminLayoutComponent,
        canActivate: [adminAuthGuard],
        children: [
            { path: 'dashboard', component: DashboardComponent},
            { path: 'appointments', component: AppointmentsComponent},
            { path: 'pets', component: PetsComponent},
        ]
    },
    { path: 'admin/login', component: AdminLoginComponent },
    {
        path: '',
        component: MainLayoutComponent,
        children: [
            {path: '', component: HomeComponent,},
            {path: 'services', component: ServicesComponent},
            {path: 'appointment', component: AppointmentComponent, canActivate: [authGuard]},
            {path: 'appointment-success', component: AppointmentSuccessComponent, canActivate: [authGuard]},
            {path: 'appointment-success/:id', component: AppointmentSuccessComponent, canActivate: [authGuard]},
            {path: 'myappointments', component: MyappointmentsComponent, canActivate: [authGuard]},
            {path: 'myappointments/:id', component: MyappointmentsDetailComponent, canActivate: [authGuard]},
            {path: 'create-pet', component: CreatePetComponent, canActivate: [authGuard]},
            {path: 'mypets', component: MyPetsComponent, canActivate: [authGuard]},
            {path: 'account/login', component: LoginComponent},
            {path: 'account/register', component: RegisterComponent},
            {path: '**', redirectTo: '', pathMatch: 'full'},
        ]
    },
    
];
