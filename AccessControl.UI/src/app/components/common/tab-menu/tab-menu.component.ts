import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AccessControlService } from 'src/app/services/access-control.service';
import { AuthService } from 'src/app/services/auth.service';

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

  userInitials: any;

  constructor(private accessService: AccessControlService,
    private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.accessService.getCurrentUser().subscribe({
      next: (response) => {
        this.userEmail = response.data.email;
        this.username = response.data.username;
        this.userInitials = this.getInitials(this.username);
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message);
      }
    });

    this.items = [
      { label: 'Dashboard', icon: 'pi pi-chart-line', routerLink: 'dashboard' },
      { label: 'Sites', icon: 'pi pi-home', routerLink: 'sites' },
      { label: 'Locks', icon: 'pi pi-lock', routerLink: 'locks' },
      { label: 'Cardholders', icon: 'pi pi-id-card', routerLink: 'cardholders' },
      { label: 'Schedules', icon: 'pi pi-calendar-clock', routerLink: 'schedules' },
      { label: 'Live Events', icon: 'pi pi-megaphone', routerLink: 'live-events' }
    ]
  }

  showSidebar() {
    this.sidebarVisible = true;
  }

  hideSidebar() {
    this.sidebarVisible = false;
  }

  getInitials(name: string): string {
    const initials = name.substring(0, 2);
    return initials.length > 2 ? initials.slice(0, 2) : initials;
  }

  logOut() {
    this.authService.removeToken();
    this.hideSidebar();
    this.router.navigate(['/login']);
  }
}