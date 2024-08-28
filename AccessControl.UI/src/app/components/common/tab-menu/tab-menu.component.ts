import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { TabMenuModule } from 'primeng/tabmenu';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'tab-menu',
  templateUrl: './tab-menu.component.html',
  styleUrl: './tab-menu.component.css'
})
export class TabMenuComponent implements OnInit {
  items: MenuItem[] | undefined;

  sidebarVisible: boolean = false;
  userEmail: any;
  username: any;

  constructor(private accessService: AccessControlService) { }

  ngOnInit() {
    this.items = [
      { label: 'Dashboard', icon: 'pi pi-chart-line', routerLink: 'dashboard' },
      { label: 'Sites', icon: 'pi pi-home', routerLink: 'sites' },
      { label: 'Locks', icon: 'pi pi-lock', routerLink: 'locks' },
      { label: 'Cardholders', icon: 'pi pi-id-card', routerLink: 'cardholders' },
      { label: 'Schedules', icon: 'pi pi-calendar-clock', routerLink: 'schedules' }
    ]
  }

  showSidebar() {
    this.accessService.getCurrentUser().subscribe({
      next: (response) => {
        this.userEmail = response.data.email;
        this.username = response.data.username;
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message);
      }
    });
    this.sidebarVisible = true;
  }

  hideSidebar() {
    this.sidebarVisible = false;
  }
}