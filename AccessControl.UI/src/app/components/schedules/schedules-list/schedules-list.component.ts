import { Component, OnInit } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { AddEvent, RemoveEvent } from '@progress/kendo-angular-grid';
import { AccessControlService } from 'src/app/services/access-control.service';
import { CreateScheduleComponent } from '../create-schedule/create-schedule.component';
import { DeleteScheduleComponent } from '../delete-schedule/delete-schedule.component';
import { EditScheduleComponent } from '../edit-schedule/edit-schedule.component';
import { SpinnerService } from 'src/app/services/spinner.service';

@Component({
  selector: 'app-schedules-list',
  templateUrl: './schedules-list.component.html',
  styleUrls: ['./schedules-list.component.css'],
  providers: [DialogService],
})
export class SchedulesListComponent implements OnInit {
  schedules: any[] = [];
  scheduleIsPresent: boolean = false;
  siteId: any = localStorage.getItem('selectedSiteId');
  siteName: any = localStorage.getItem('selectedSiteName');

  constructor(
    private accessService: AccessControlService,
    public spinner: SpinnerService,
    private dialog: DialogService
  ) {}

  ngOnInit(): void {
    this.spinner.show();
    this.accessService.getWithParams(`api/schedules`, '').subscribe({
      next: (response) => {
        this.schedules = response.data;
        this.scheduleIsPresent = true;
        this.spinner.hide();
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
        this.scheduleIsPresent = false;
        this.spinner.hide();
      },
    });
  }

  showSchedules(site: any) {
    this.siteName = site.displayName;
    this.siteId = site.siteId;

    let request =
      this.siteName !== 'All Sites'
        ? {
            siteId: this.siteId,
          }
        : '';

    this.accessService.getWithParams(`api/schedules`, request).subscribe({
      next: (response) => {
        this.schedules = response.data;
        this.scheduleIsPresent = true;
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
        this.scheduleIsPresent = false;
      },
    });
  }

  onCreate() {
    const ref = this.dialog.open(CreateScheduleComponent, {
      header: 'Create Schedule',
      width: '610px',
      height: '680px',
      baseZIndex: 10000,
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEdit(schedule: any) {
    const ref = this.dialog.open(EditScheduleComponent, {
      header: `Edit Schedule from ${schedule.siteName}`,
      width: '610px',
      height: '600px',
      baseZIndex: 10000,
      data: {
        schedule: schedule,
      },
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
        siteName: schedule.siteName,
      },
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }
}
