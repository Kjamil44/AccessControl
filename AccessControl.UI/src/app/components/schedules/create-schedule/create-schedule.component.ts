import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DateInputFillMode } from '@progress/kendo-angular-dateinputs';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-create-schedule',
  templateUrl: './create-schedule.component.html',
  styleUrls: ['./create-schedule.component.css']
})
export class CreateScheduleComponent extends DialogContentBase {

  siteId: any
  formGroup: FormGroup;
  daysInWeek: any[] = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"]
  daysValue: any[] = []
  dateStartValue: Date = new Date(2022, 11, 11, 11);
  dateEndValue: Date = new Date(2022, 11, 21, 11);
  dateFormat = "dd/MM/yyyy HH:mm"
  fillMode: DateInputFillMode = "outline";

  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
    this.formGroup = new FormGroup({
      displayName: new FormControl(),
      days: new FormControl()
    });
  }

  getDays(args: any) {
    this.daysValue.push(args);
  }

  createSchedule() {
    const data = {
      "siteId": this.siteId,
      "displayName": this.formGroup.value.displayName,
      "listOfDays": this.daysValue,
      "startTime": this.dateStartValue.toUTCString(),
      "endTime": this.dateEndValue.toUTCString()
    }
    this.accessService.create(`api/schedules/create`, data)
      .subscribe({
        next: data => {
          this.accessService.createSuccessNotification("Schedule created successfully!")
          this.closeCreateDialog()
        },
        error: error => {
          this.accessService.createErrorNotification(error.message)
          this.closeCreateDialog()
        }
      })
  }

  closeCreateDialog() {
    this.dialog.close();
  }
}
