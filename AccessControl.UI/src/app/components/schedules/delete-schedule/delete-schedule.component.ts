import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';
import { SpinnerService } from 'src/app/services/spinner.service';

@Component({
  selector: 'app-delete-schedule',
  templateUrl: './delete-schedule.component.html',
  styleUrls: ['./delete-schedule.component.css'],
})
export class DeleteScheduleComponent implements OnInit {
  schedule: any;
  siteName: any;

  constructor(
    private dialogref: DynamicDialogRef,
    private config: DynamicDialogConfig,
    public spinner: SpinnerService,
    private accessService: AccessControlService
  ) {}

  ngOnInit(): void {
    this.schedule = this.config.data.schedule;
    this.siteName = this.schedule.siteName;
  }

  deleteSchedule() {
    this.spinner
      .with(
        this.accessService.delete(`api/schedules`, this.schedule.scheduleId)
      )
      .subscribe({
        next: (data) => {
          this.accessService.createSuccessNotification(
            'Schedule deleted successfully!'
          );
          this.closeDeleteDialog();
        },
        error: (err: Error) => {
          this.accessService.createErrorNotification(err.message);
          this.closeDeleteDialog();
        },
      });
  }

  closeDeleteDialog() {
    this.dialogref.close();
  }
}
