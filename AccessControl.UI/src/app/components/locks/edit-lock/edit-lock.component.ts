import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-edit-lock',
  templateUrl: './edit-lock.component.html',
  styleUrls: ['./edit-lock.component.css']
})
export class EditLockComponent extends DialogContentBase implements OnInit {
  site: any
  lock: any
  nameValue: any
  formGroup: FormGroup;

  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
    this.formGroup = new FormGroup({
      displayName: new FormControl(),
    });
  }
  ngOnInit(): void {
    this.nameValue = this.lock.displayName
  }

  editLock() {
    const data = {
      "displayName": this.nameValue
    }
    this.accessService.update(`api/locks/update`, this.lock.lockId, data).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Lock edited successfully!")
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
