import { Component, OnInit } from '@angular/core';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-delete-lock',
  templateUrl: './delete-lock.component.html',
  styleUrls: ['./delete-lock.component.css']
})
export class DeleteLockComponent extends DialogContentBase{
  site: any
  lock: any
  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
  }

  deleteLock() {
    this.accessService.delete(`api/locks/delete`, this.lock.lockId).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Lock deleted successfully!")
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
