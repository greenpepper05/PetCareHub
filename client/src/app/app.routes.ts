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
import { AppointmentDetailComponent } from './features/admin/appointments/appointment-detail/appointment-detail.component';
import { TestErrorComponent } from './features/test-error/test-error.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';
import { PetServiceComponent } from './features/admin/pet-service/pet-service.component';
import { PetServiceDetailComponent } from './features/admin/pet-service/pet-service-detail/pet-service-detail.component';
import { ServicesDetailComponent } from './features/services/services-detail/services-detail.component';
import { ManageServicesComponent } from './features/admin/manage-services/manage-services.component';

export const routes: Routes = [
    {
        path: 'admin',
        component: AdminLayoutComponent,
        canActivate: [adminAuthGuard],
        children: [
            { path: 'dashboard', component: DashboardComponent},
            { path: 'appointments', component: AppointmentsComponent},
            { path: 'appointment/:id', component: AppointmentDetailComponent},
            { path: 'pets', component: PetsComponent},
            { path: 'pet-service', component: PetServiceComponent},
            { path: 'pet-service/:id', component: PetServiceDetailComponent},
            { path: 'manage-services', component: ManageServicesComponent},

        ]
    },
    { path: 'admin/login', component: AdminLoginComponent },
    {
        path: '',
        component: MainLayoutComponent,
        children: [
            {path: '', component: HomeComponent,},
            {path: 'services', component: ServicesComponent},
            {path: 'services-detail/:id', component: ServicesDetailComponent},
            {path: 'appointment', component: AppointmentComponent, canActivate: [authGuard]},
            {path: 'appointment-success', component: AppointmentSuccessComponent, canActivate: [authGuard]},
            {path: 'appointment-success/:id', component: AppointmentSuccessComponent, canActivate: [authGuard]},
            {path: 'myappointments', component: MyappointmentsComponent, canActivate: [authGuard]},
            {path: 'myappointments/:id', component: MyappointmentsDetailComponent, canActivate: [authGuard]},
            {path: 'create-pet', component: CreatePetComponent, canActivate: [authGuard]},
            {path: 'mypets', component: MyPetsComponent, canActivate: [authGuard]},
            {path: 'account/login', component: LoginComponent},
            {path: 'account/register', component: RegisterComponent},
            {path: 'test-error', component: TestErrorComponent},
            {path: 'not-found', component: NotFoundComponent},
            {path: 'server-error', component: ServerErrorComponent},
            {path: '**', redirectTo: '', pathMatch: 'full'},
        ]
    },
    
];
