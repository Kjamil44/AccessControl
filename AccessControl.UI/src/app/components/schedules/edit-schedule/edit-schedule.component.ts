import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DateInputFillMode } from '@progress/kendo-angular-dateinputs';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-edit-schedule',
  templateUrl: './edit-schedule.component.html',
  styleUrls: ['./edit-schedule.component.css']
})
export class EditScheduleComponent extends DialogContentBase implements OnInit {
  site: any
  schedule: any
  nameValue: any
  daysInWeek: any[] = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"]
  daysValue: any[] = []
  dateStartValue: Date = new Date(2022, 11, 11, 11);
  dateEndValue: Date = new Date(2022, 11, 21, 11);
  dateFormat = "dd/MM/yyyy HH:mm"
  fillMode: DateInputFillMode = "outline";

  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
  }

  ngOnInit(): void {
    this.nameValue = this.schedule.displayName;
    this.dateStartValue = new Date(this.schedule.startTime);
    this.dateEndValue = new Date(this.schedule.endTime);
  }

  getDays(args: any) {
    this.daysValue.push(args);
  }

  editSchedule() {
    const data = {
      "displayName": this.nameValue,
      "listOfDays": this.daysValue,
      "startTime": this.dateStartValue,
      "endTime": this.dateEndValue
    }

    this.accessService.update(`api/schedules/update`, this.schedule.scheduleId, data).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Schedule edited successfully!")
        this.closeEditDialog()

      },
      error: error => {
        this.accessService.createErrorNotification(error.message)
        this.closeEditDialog()
      }
    })
  }

  closeEditDialog() {
    this.dialog.close();
  }
}
