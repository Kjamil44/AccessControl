import { Component, OnInit } from '@angular/core';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-delete-cardholder',
  templateUrl: './delete-cardholder.component.html',
  styleUrls: ['./delete-cardholder.component.css']
})
export class DeleteCardholderComponent extends DialogContentBase{
  site: any
  cardholder: any
  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
  }

  deleteCardholder() {
    this.accessService.delete(`api/cardholders/delete`, this.cardholder.cardholderId).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Cardholder deleted successfully!")
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
