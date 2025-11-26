// product-overview.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

interface OrgNode {
  label: string;
  icon?: string;
  expanded?: boolean;
  children?: OrgNode[];
}

@Component({
  selector: 'app-product-overview',
  templateUrl: './product-overview.component.html',
  styleUrls: ['./product-overview.component.css'],
})
export class ProductOverviewComponent implements OnInit {
  // replace with your real logo path if different
  logoSrc = 'assets/images/short-logo.png';

  constructor(private router: Router) {}

  orgData: OrgNode[] = [
    {
      label: 'Access Control',
      icon: 'pi pi-shield',
      expanded: true,
      children: [
        {
          label: 'Site',
          icon: 'pi pi-building',
          expanded: true,
          children: [
            {
              label: 'Lock',
              expanded: true,
              icon: 'pi pi-lock',
              children: [
                { label: 'Cardholder', icon: 'pi pi-id-card' },
                { label: 'Schedule', icon: 'pi pi-calendar-clock' },
              ],
            },
          ],
        },
      ],
    },
  ];

  featureBullets = [
    {
      icon: 'pi pi-lock-open',
      title: 'Granular Access',
      desc: 'Assign & revoke permissions per site, lock, or person.',
    },
    {
      icon: 'pi pi-history',
      title: 'Live Events',
      desc: 'Real-time logs to audit activity as it happens.',
    },
    {
      icon: 'pi pi-calendar',
      title: 'Smart Schedules',
      desc: 'Time-bound, recurring, and exception windows.',
    },
    {
      icon: 'pi pi-shield',
      title: 'Secure by Design',
      desc: 'Strong defaults, role-based access, safe operations.',
    },
  ];

  ngOnInit(): void {}

  onSiteCreate() {
    this.router.navigate([`/sites`]);
  }
}
