import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DialogContentBase, DialogRef, DialogService } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-delete-site',
  templateUrl: './delete-site.component.html',
  styleUrls: ['./delete-site.component.css']
})
export class DeleteSiteComponent extends DialogContentBase {

  site: any;

  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
  }

  deleteSite() {
    this.accessService.delete('api/sites/delete', this.site.siteId).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Site deleted successfully!")
        this.closeDeleteDialog()
      },
      error: error => {
        this.accessService.createErrorNotification("Failed to delete Site: Lock, Cardholder or Schedule still exist on Site!")
        this.closeDeleteDialog()
      }
    })
  }

  closeDeleteDialog() {
    this.dialog.close();
  }

}
