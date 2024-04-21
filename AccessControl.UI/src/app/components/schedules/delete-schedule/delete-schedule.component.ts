import { Component, OnInit } from '@angular/core';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-delete-schedule',
  templateUrl: './delete-schedule.component.html',
  styleUrls: ['./delete-schedule.component.css']
})
export class DeleteScheduleComponent extends DialogContentBase {
  site: any
  schedule: any
  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
  }

  deleteSchedule() {
    this.accessService.delete(`api/schedules/delete`, this.schedule.scheduleId).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Schedule deleted successfully!")
        this.closeDeleteDialog()
      },
      error: error => {
        this.accessService.createErrorNotification(error.message)
        this.closeDeleteDialog()
      }
    })
  }

  closeDeleteDialog() {
    this.dialog.close();
  }
}
