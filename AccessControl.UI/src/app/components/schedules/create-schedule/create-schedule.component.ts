import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-create-schedule',
  templateUrl: './create-schedule.component.html',
  styleUrls: ['./create-schedule.component.css']
})
export class CreateScheduleComponent implements OnInit {
  siteId: any;
  formGroup: FormGroup;

  daysInWeek: any[] = [
    { name: "Monday", value: "Monday" },
    { name: "Tuesday", value: "Tuesday" },
    { name: "Wednesday", value: "Wednesday" },
    { name: "Thursday", value: "Thursday" },
    { name: "Friday", value: "Friday" },
    { name: "Saturday", value: "Saturday" },
    { name: "Sunday", value: "Sunday" }
  ];

  constructor(
    private accessService: AccessControlService,
    private dialogref: DynamicDialogRef,
    private config: DynamicDialogConfig
  ) {
    this.formGroup = new FormGroup({
      displayName: new FormControl(),
      days: new FormControl([]),
      startTime: new FormControl(),
      endTime: new FormControl()
    });
  }

  ngOnInit(): void {
    this.siteId = this.config.data.siteId;
  }

  createSchedule() {
    const selectedDays = this.formGroup.value.days.map((day: any) => day.value);
    const data = {
      "siteId": this.siteId,
      "displayName": this.formGroup.value.displayName,
      "listOfDays": selectedDays,
      "startTime": this.formGroup.value.startTime,
      "endTime": this.formGroup.value.endTime
    };
    this.accessService.create(`api/schedules/create`, data)
      .subscribe({
        next: data => {
          this.accessService.createSuccessNotification("Schedule created successfully!");
          this.closeCreateDialog();
        },
        error: error => {
          this.accessService.createErrorNotification(error.message);
          this.closeCreateDialog();
        }
      });
  }

  closeCreateDialog() {
    this.dialogref.close();
  }
}
