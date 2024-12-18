import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { NavigationModule } from '@progress/kendo-angular-navigation';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SitesListComponent } from './components/sites/sites-list/sites-list.component';
import { GridModule } from '@progress/kendo-angular-grid';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { DialogsModule } from '@progress/kendo-angular-dialog';
import { CreateSiteComponent } from './components/sites/create-site/create-site.component';
import { EditSiteComponent } from './components/sites/edit-site/edit-site.component';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { DeleteSiteComponent } from './components/sites/delete-site/delete-site.component';
import { LocksListComponent } from './components/locks/locks-list/locks-list.component';
import { MenuModule } from '@progress/kendo-angular-menu';
import { CreateLockComponent } from './components/locks/create-lock/create-lock.component';
import { DeleteLockComponent } from './components/locks/delete-lock/delete-lock.component';
import { EditLockComponent } from './components/locks/edit-lock/edit-lock.component';
import { AllowedUsersLockComponent } from './components/locks/allowed-users-lock/allowed-users-lock.component';
import { LabelModule } from '@progress/kendo-angular-label';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { CardholdersListComponent } from './components/cardholders/cardholders-list/cardholders-list.component';
import { CreateCardholderComponent } from './components/cardholders/create-cardholder/create-cardholder.component';
import { EditCardholderComponent } from './components/cardholders/edit-cardholder/edit-cardholder.component';
import { DeleteCardholderComponent } from './components/cardholders/delete-cardholder/delete-cardholder.component';
import { SchedulesListComponent } from './components/schedules/schedules-list/schedules-list.component';
import { CreateScheduleComponent } from './components/schedules/create-schedule/create-schedule.component';
import { EditScheduleComponent } from './components/schedules/edit-schedule/edit-schedule.component';
import { DeleteScheduleComponent } from './components/schedules/delete-schedule/delete-schedule.component';
import { MatIconModule } from '@angular/material/icon';
import { NotificationModule } from '@progress/kendo-angular-notification';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { TabMenuModule } from 'primeng/tabmenu';
import { TabMenuComponent } from './components/common/tab-menu/tab-menu.component';
import { MenubarModule } from 'primeng/menubar';
import { DropdownModule } from 'primeng/dropdown';
import { SiteSelectorComponent } from './components/common/site-selector/site-selector.component';
import { CardModule } from 'primeng/card';
import { CalendarModule } from 'primeng/calendar';
import { MultiSelectModule } from 'primeng/multiselect';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ChartModule } from 'primeng/chart';
import { OrganizationChartModule } from 'primeng/organizationchart';
import { MessageService } from 'primeng/api';
import { PasswordModule } from 'primeng/password';
import { LoginComponent } from './components/common/login/login.component';
import { DividerModule } from 'primeng/divider';
import { RegisterComponent } from './components/common/register/register.component';
import { AuthInterceptor } from './services/auth-interceptor';
import { AvatarModule } from 'primeng/avatar';
import { AvatarGroupModule } from 'primeng/avatargroup';
import { SidebarModule } from 'primeng/sidebar';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { FooterComponent } from './components/common/footer/footer.component';

@NgModule({
  declarations: [
    AppComponent,
    SitesListComponent,
    CreateSiteComponent,
    EditSiteComponent,
    DeleteSiteComponent,
    LocksListComponent,
    CreateLockComponent,
    DeleteLockComponent,
    EditLockComponent,
    AllowedUsersLockComponent,
    CardholdersListComponent,
    CreateCardholderComponent,
    EditCardholderComponent,
    DeleteCardholderComponent,
    SchedulesListComponent,
    CreateScheduleComponent,
    EditScheduleComponent,
    DeleteScheduleComponent,
    TabMenuComponent,
    SiteSelectorComponent,
    DashboardComponent,
    LoginComponent,
    RegisterComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    NavigationModule,
    BrowserAnimationsModule,
    GridModule,
    HttpClientModule,
    ButtonsModule,
    DialogsModule,
    DropDownsModule,
    ReactiveFormsModule,
    InputsModule,
    MenuModule,
    LabelModule,
    FormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    NotificationModule,
    ButtonModule,
    TableModule,
    DynamicDialogModule,
    DialogModule,
    InputTextModule,
    ConfirmDialogModule,
    ToastModule,
    TabMenuModule,
    MenubarModule,
    DropdownModule,
    CardModule,
    CalendarModule,
    MultiSelectModule,
    ChartModule,
    OrganizationChartModule,
    PasswordModule,
    DividerModule,
    AvatarGroupModule,
    AvatarModule,
    SidebarModule,
    RadioButtonModule,
    ToggleButtonModule
  ],
  providers: [
    MessageService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }

