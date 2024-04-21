import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-create-lock',
  templateUrl: './create-lock.component.html',
  styleUrls: ['./create-lock.component.css']
})
export class CreateLockComponent extends DialogContentBase {
  siteId: any
  formGroup: FormGroup;

  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
    this.formGroup = new FormGroup({
      displayName: new FormControl(),
    });
  }
  createLock() {
    const data = {
      "siteId": this.siteId,
      "displayName": this.formGroup.value.displayName,
    }
    console.log(data)
    this.accessService.create(`api/locks/create`, data)
      .subscribe({
        next: data => {
          this.accessService.createSuccessNotification("Lock created successfully!")
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
