import { Component, OnInit } from '@angular/core';
import { TreeNode } from 'primeng/api';
import { ChartOptions, ChartData } from 'chart.js';
import { AccessControlService } from 'src/app/services/access-control.service';
import { CreateSiteComponent } from '../sites/create-site/create-site.component';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  providers: [DialogService]
})
export class DashboardComponent implements OnInit {
  data!: TreeNode[];

  siteChartData: any;
  lockChartData: any;
  cardholderTimeChartData: any;
  chartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false
  };

  sitesCount!: number;
  locksCount!: number;
  cardholderCount!: number;
  scheduleCount!: number;
  accessCount!: number;

  showNoSitesDialog: boolean = false;

  constructor(private accessService: AccessControlService, private dialog: DialogService) {}

  ngOnInit() {
    this.accessService.getForDashboard('info').subscribe({
      next: (response) => {
        this.updateDashboardData(response.data);
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
      }
    });

    this.data = [
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
              { label: 'Schedule', icon: 'pi pi-calendar-clock' }
            ]
          }
        ]
      }
    ];
  }

  private updateDashboardData(data: any) {
    this.sitesCount = data.numberOfSites;
    this.locksCount = data.numberOfLocks;
    this.cardholderCount = data.numberOfCardholders;
    this.scheduleCount = data.numberOfSchedules;
    this.accessCount = data.cardholdersWithAccess;

    this.showNoSitesDialog = this.sitesCount == 0;

    const siteLabels = data.allSites.map((site: any) => site.displayName);
    const siteData = data.allSites.map((site: any) => site.data);
    const siteColors = data.allSites.map((site: any) => site.backgroundColor);

    this.siteChartData = {
      labels: siteLabels,
      datasets: [{
        data: siteData,
        backgroundColor: siteColors
      }]
    };

    const lockLabels = data.allLocksBySite.map((lock: any) => lock.displayName);
    const lockData = data.allLocksBySite.map((lock: any) => lock.data);
    const lockColors = data.allLocksBySite.map((lock: any) => lock.backgroundColor);

    this.lockChartData = {
      labels: lockLabels,
      datasets: [{
        data: lockData,
        backgroundColor: lockColors
      }]
    };

    const lastSixMonths = data.cardholdersInLastSixMonths.map((x: any) => x.monthName);
    const numOfCardholders = data.cardholdersInLastSixMonths.map((x: any) => x.cardholdersCount);

    this.cardholderTimeChartData = {
      labels: lastSixMonths,
      datasets: [
        {
          label: 'Cardholders',
          data: numOfCardholders,
          fill: false,
          borderColor: '#4bc0c0'
        }
      ]
    };
  }

  navigateToCreateSite(){
    const ref = this.dialog.open(CreateSiteComponent, {
      header: 'Create Site',
      width: '600px',
      height: '250px',
      baseZIndex: 10000
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }
}
