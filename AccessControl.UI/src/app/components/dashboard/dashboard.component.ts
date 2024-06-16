import { Component, OnInit } from '@angular/core';
import { TreeNode } from 'primeng/api';
import { ChartOptions, ChartData } from 'chart.js';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
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

  cardholderCount!: number;
  scheduleCount!: number;
  accessCount!: number;

  constructor(private accessService: AccessControlService) {}

  ngOnInit() {
    this.accessService.getForDashboard('info').subscribe({
      next: (response) => {
        this.updateDashboardData(response.data);
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message);
      }
    });

    this.data = [
      {
        label: 'Site',
        icon: 'pi pi-home',
        children: [
          {
            label: 'Lock',
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
    this.cardholderCount = data.numberOfCardholders;
    this.scheduleCount = data.numberOfSchedules;
    this.accessCount = data.cardholdersWithAccess;

    const siteLabels = data.allSites.map((site: any) => site.displayName);
    const siteData = data.allSites.map(() => Math.floor(Math.random() * 100)); // Mock data for example

    this.siteChartData = {
      labels: siteLabels,
      datasets: [{
        data: siteData,
        backgroundColor: siteData.map(() => this.generateRandomColor())
      }]
    };

    const lockLabels = data.allLocksBySite.map((lock: any) => lock.displayName);
    const lockData = data.allLocksBySite.map(() => Math.floor(Math.random() * 100)); // Mock data for example

    this.lockChartData = {
      labels: lockLabels,
      datasets: [{
        data: lockData,
        backgroundColor: lockData.map(() => this.generateRandomColor())
      }]
    };

    this.cardholderTimeChartData = {
      labels: ['January', 'February', 'March', 'April', 'May', 'June'],
      datasets: [
        {
          label: 'Cardholders',
          data: [65, 59, 80, 81, 56, 55], // Example data, you can update this with real data if available
          fill: false,
          borderColor: '#4bc0c0'
        }
      ]
    };
  }

  private generateRandomColor(): string {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }
}
