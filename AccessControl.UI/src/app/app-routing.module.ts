import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CardholdersListComponent } from './components/cardholders/cardholders-list/cardholders-list.component';
import { LocksListComponent } from './components/locks/locks-list/locks-list.component';
import { SchedulesListComponent } from './components/schedules/schedules-list/schedules-list.component';
import { CreateSiteComponent } from './components/sites/create-site/create-site.component';
import { SitesListComponent } from './components/sites/sites-list/sites-list.component';

const routes: Routes = [
  {
    path: 'sites',
    component: SitesListComponent
  },
  {
    path: 'locks',
    component: LocksListComponent
  },
  {
    path: 'cardholders',
    component: CardholdersListComponent
  },
  {
    path: 'schedules',
    component: SchedulesListComponent
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
