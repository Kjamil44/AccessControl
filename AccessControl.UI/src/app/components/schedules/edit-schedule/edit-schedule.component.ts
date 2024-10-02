import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-edit-schedule',
  templateUrl: './edit-schedule.component.html',
  styleUrls: ['./edit-schedule.component.css']
})
export class EditScheduleComponent implements OnInit {
  schedule: any

  daysInWeek: any[] = [
    { name: "Monday", value: "Monday" },
    { name: "Tuesday", value: "Tuesday" },
    { name: "Wednesday", value: "Wednesday" },
    { name: "Thursday", value: "Thursday" },
    { name: "Friday", value: "Friday" },
    { name: "Saturday", value: "Saturday" },
    { name: "Sunday", value: "Sunday" }
  ];

  formGroup: FormGroup;

  isTemporary: boolean = false;

  constructor(private dialogref: DynamicDialogRef,
    private accessService: AccessControlService,
    private config: DynamicDialogConfig) {
    this.formGroup = new FormGroup({
      displayName: new FormControl(),
      days: new FormControl([]),
      startTime: new FormControl(),
      endTime: new FormControl(),
      isTemporary: new FormControl<boolean>(false)
    });
  }

  ngOnInit(): void {
    this.schedule = this.config.data.schedule;

    const selectedDays = this.schedule.listOfDays.map((day: string) => {
      return this.daysInWeek.find((x: any) => x.name === day);
    }).filter((day: any) => day !== undefined);

    this.formGroup.controls['displayName'].setValue(this.schedule.displayName);
    this.formGroup.controls['days'].setValue(selectedDays);
    this.formGroup.controls['startTime'].setValue(new Date(this.schedule.startTime));
    this.formGroup.controls['endTime'].setValue(new Date(this.schedule.endTime));
    this.formGroup.controls['isTemporary'].setValue(this.schedule.type);
    this.isTemporary = this.schedule.type === 'Temporary';
    this.formGroup.controls['isTemporary'].disable();
  }

  editSchedule() {
    const selectedDays = this.formGroup.value.days.map((day: any) => day.value);
    const data = {
      "displayName": this.formGroup.value.displayName,
      "listOfDays": selectedDays,
      "startTime": this.formGroup.value.startTime,
      "endTime": this.formGroup.value.endTime
    };

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
    this.dialogref.close();
  }
}
