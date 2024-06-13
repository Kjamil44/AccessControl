import { Component, OnInit } from '@angular/core';
import { TreeNode } from 'primeng/api';
import { ChartOptions, ChartData } from 'chart.js';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  data!: TreeNode[];

  siteChartData!: ChartData<'pie'>;
  lockChartData!: ChartData<'pie'>;
  cardholderChartData!: ChartData<'pie'>;
  scheduleChartData!: ChartData<'pie'>;
  chartOptions!: ChartOptions<'pie'>;

  constructor(private accessService: AccessControlService) {
    this.accessService.getForDashboard(`api/dashboard/info`).subscribe({
      next: (response) => {
          console.log(response);
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message)
      }
    });
  }

  ngOnInit(): void {
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
    
    this.siteChartData = {
      labels: ['Site 1', 'Site 2', 'Site 3'],
      datasets: [
        {
          data: [300, 500, 200],
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
          hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
        }
      ]
    };

    this.lockChartData = {
      labels: ['Lock 1', 'Lock 2', 'Lock 3'],
      datasets: [
        {
          data: [100, 150, 75],
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
          hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
        }
      ]
    };

    this.cardholderChartData = {
      labels: ['Cardholder 1', 'Cardholder 2', 'Cardholder 3'],
      datasets: [
        {
          data: [20, 40, 10],
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
          hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
        }
      ]
    };

    this.scheduleChartData = {
      labels: ['Schedule 1', 'Schedule 2', 'Schedule 3'],
      datasets: [
        {
          data: [10, 15, 5],
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
          hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
        }
      ]
    };

    this.chartOptions = {
      plugins: {
        legend: {
          display: true,
          position: 'bottom',
          labels: {
            color: '#33333' /* Dark gray text color for legend */
          }
        }
      }
    };
  }
}