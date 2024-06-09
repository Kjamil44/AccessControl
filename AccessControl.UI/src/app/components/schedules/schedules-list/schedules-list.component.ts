import { Component, OnInit } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { AddEvent, RemoveEvent } from '@progress/kendo-angular-grid';
import { AccessControlService } from 'src/app/services/access-control.service';
import { CreateScheduleComponent } from '../create-schedule/create-schedule.component';
import { DeleteScheduleComponent } from '../delete-schedule/delete-schedule.component';
import { EditScheduleComponent } from '../edit-schedule/edit-schedule.component';
import { MatIconModule } from '@angular/material/icon'

@Component({
  selector: 'app-schedules-list',
  templateUrl: './schedules-list.component.html',
  styleUrls: ['./schedules-list.component.css'],
  providers: [DialogService]
})
export class SchedulesListComponent implements OnInit {
  sites: any[] = []
  schedules: any[] = []
  scheduleIsPresent: boolean = false;
  siteId: any = localStorage.getItem("selectedSiteId")
  siteName: any = localStorage.getItem("selectedSiteName")

  constructor(private accessService: AccessControlService, private dialog: DialogService) { }

  ngOnInit(): void {
    this.accessService.get('api/sites').subscribe({
      next: (response) => {
        this.sites = response.data;
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message)
      }
    })

    this.accessService.get(`api/schedules/site/${this.siteId}`).subscribe({
      next: (response) => {
        this.schedules = response.data;
        this.scheduleIsPresent = true;
      },
      error: (response) => {
        this.scheduleIsPresent = false;
      }
    })
  }

  showSchedules(site: any) {
    this.siteName = site.displayName;
    this.siteId = site.siteId;

    this.accessService.get(`api/schedules/site/${this.siteId}`).subscribe({
      next: (response) => {
        this.schedules = response.data;
        this.scheduleIsPresent = true;
      },
      error: (response) => {
        this.scheduleIsPresent = false;
      }
    })
  }

  onCreate() {
    const ref = this.dialog.open(CreateScheduleComponent, {
      header: 'Create Schedule',
      width: '610px',
      height: '500px',
      baseZIndex: 10000,
      data: {
        siteId: this.siteId
      }
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEdit(schedule: any) {
    const ref = this.dialog.open(EditScheduleComponent, {
      header: `Edit Schedule from ${this.siteName}`,
      width: '610px',
      height: '520px',
      baseZIndex: 10000,
      data: {
        schedule: schedule
      }
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onDelete(schedule: any) {
    const ref = this.dialog.open(DeleteScheduleComponent, {
      header: 'Delete Schedule',
      width: '470px',
      height: '250px',
      baseZIndex: 10000,
      data: {
        schedule: schedule,
        siteName: this.siteName
      }
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

}
