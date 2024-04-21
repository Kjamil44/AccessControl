import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { delay } from 'rxjs';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-edit-site',
  templateUrl: './edit-site.component.html',
  styleUrls: ['./edit-site.component.css']
})
export class EditSiteComponent extends DialogContentBase implements OnInit {
  site: any;
  submitted = false;
  nameValue: any

  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);

  }

  ngOnInit(): void {
    this.nameValue = this.site.displayName;
  }

  editSite() {
    const data = {
      "displayName": this.nameValue
    }

    this.accessService.update('api/sites/update', this.site.siteId, data).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Site edited successfully!")
        this.closeCreateDialog()
      },
      error: error => {
        this.accessService.createErrorNotification(`Failed to Update Site: ${error.message}`)
        this.closeCreateDialog()
      }
    })
  }

  closeCreateDialog() {
    this.dialog.close();
  }

}
