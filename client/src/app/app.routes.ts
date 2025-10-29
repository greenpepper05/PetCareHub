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
import { ProcedureComponent } from './features/admin/pet-service/procedure/procedure.component';
import { ServiceDetailComponent } from './features/admin/manage-services/service-detail/service-detail.component';
import { PetDetailsComponent } from './features/admin/pets/pet-details/pet-details.component';
import { SuperadminLoginComponent } from './features/superadmin/superadmin-login/superadmin-login.component';
import { LayoutComponent } from './features/superadmin/layout/layout.component';
import { superadminAuthGuard } from './core/guards/superadmin-auth-guard';
import { SuperadminDashboardComponent } from './features/superadmin/superadmin-dashboard/superadmin-dashboard.component';
import { SuperadminClinicComponent } from './features/superadmin/superadmin-clinic/superadmin-clinic.component';
import { SuperadminUsersComponent } from './features/superadmin/superadmin-users/superadmin-users.component';
import { SuperadminSettingsComponent } from './features/superadmin/superadmin-settings/superadmin-settings.component';
import { UserUpdateComponent } from './features/superadmin/superadmin-users/user-update/user-update.component';
import { ClinicDetailComponent } from './features/superadmin/superadmin-clinic/clinic-detail/clinic-detail.component';

export const routes: Routes = [
    {
        path: 'admin',
        component: AdminLayoutComponent,
        canActivate: [adminAuthGuard],
        children: [
            { path: '', component: DashboardComponent},
            { path: 'dashboard', component: DashboardComponent},
            { path: 'appointments', component: AppointmentsComponent},
            { path: 'appointment/:id', component: AppointmentDetailComponent},
            { path: 'pets', component: PetsComponent},
            { path: 'pets/:id', component: PetDetailsComponent},
            { path: 'service-record', component: PetServiceComponent},
            { path: 'service-record/:id', component: PetServiceDetailComponent},
            { path: 'service-record/:id/procedure', component: ProcedureComponent},
            { path: 'manage-services', component: ManageServicesComponent},
            { path: 'manage-services/:id', component: ServiceDetailComponent},
        ]
    },
    { path: 'admin/login', component: AdminLoginComponent },
    {
        path: 'superadmin',
        component: LayoutComponent,
        canActivate: [superadminAuthGuard],
        children: [
            { path: '', component: SuperadminDashboardComponent},
            { path: 'dashboard', component: SuperadminDashboardComponent},
            { path: 'clinics', component: SuperadminClinicComponent},
            { path: 'clinics/:id', component: ClinicDetailComponent},
            { path: 'users', component: SuperadminUsersComponent},
            { path: 'users/:id', component: UserUpdateComponent},
            { path: 'settings', component: SuperadminSettingsComponent},
        ]
    },
    { path: 'superadmin/login', component: SuperadminLoginComponent},
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
