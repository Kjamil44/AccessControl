import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AccessControlService } from 'src/app/services/access-control.service';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-edit-lock',
  templateUrl: './edit-lock.component.html',
  styleUrls: ['./edit-lock.component.css']
})
export class EditLockComponent implements OnInit {
  lock: any
  formGroup: FormGroup;

  constructor(private dialogref: DynamicDialogRef,
    private accessService: AccessControlService,
    private config: DynamicDialogConfig) {
    this.formGroup = new FormGroup({
      displayName: new FormControl(),
    });
  }
  ngOnInit(): void {
    this.lock = this.config.data.lock;
    this.formGroup.controls['displayName'].setValue(this.lock.displayName);
  }

  editLock() {
    const data = {
      "displayName": this.formGroup.value.displayName
    }
    this.accessService.update(`api/locks`, this.lock.lockId, data).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Lock updated successfully!")
        this.closeEditDialog()
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message)
        this.closeEditDialog()
      }
    })
  }

  closeEditDialog() {
    this.dialogref.close();
  }

}
