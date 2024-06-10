import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { TabMenuModule } from 'primeng/tabmenu';

@Component({
  selector: 'tab-menu',
  templateUrl: './tab-menu.component.html',
  styleUrl: './tab-menu.component.css'
})
export class TabMenuComponent implements OnInit {
  items: MenuItem[] | undefined;

  ngOnInit() {
      this.items = [
          { label: 'Dashboard', icon: 'pi pi-chart-line', routerLink: '/' },
          { label: 'Sites', icon: 'pi pi-home', routerLink: 'sites' },
          { label: 'Locks', icon: 'pi pi-lock', routerLink: 'locks' },
          { label: 'Cardholders', icon: 'pi pi-id-card', routerLink: 'cardholders' },
          { label: 'Schedules', icon: 'pi pi-calendar-clock', routerLink: 'schedules' }
      ]
  }
}