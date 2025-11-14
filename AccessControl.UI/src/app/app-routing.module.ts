import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CardholdersListComponent } from './components/cardholders/cardholders-list/cardholders-list.component';
import { LocksListComponent } from './components/locks/locks-list/locks-list.component';
import { SchedulesListComponent } from './components/schedules/schedules-list/schedules-list.component';
import { CreateSiteComponent } from './components/sites/create-site/create-site.component';
import { SitesListComponent } from './components/sites/sites-list/sites-list.component';
import { AllowedUsersLockComponent } from './components/locks/allowed-users-lock/allowed-users-lock.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/common/login/login.component';
import { RegisterComponent } from './components/common/register/register.component';
import { LiveEventsComponent } from './components/live-events/live-events.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'dashboard',
    component: DashboardComponent
  },
  {
    path: 'sites',
    component: SitesListComponent
  },
  {
    path: 'locks',
    component: LocksListComponent
  },
  {
    path: 'locks/:id/edit-access',
    component: AllowedUsersLockComponent
  },
  {
    path: 'cardholders',
    component: CardholdersListComponent
  },
  {
    path: 'schedules',
    component: SchedulesListComponent
  },
  {
    path: 'live-events',
    component: LiveEventsComponent
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
