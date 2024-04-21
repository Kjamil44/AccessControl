import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-create-site',
  templateUrl: './create-site.component.html',
  styleUrls: ['./create-site.component.css']
})
export class CreateSiteComponent extends DialogContentBase {
  formGroup: FormGroup;

  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
    this.formGroup = new FormGroup({
      displayName: new FormControl(),
    });
  }
  createSite() {
    const data = {
      "displayName": this.formGroup.value.displayName,
    }

    this.accessService.create('api/sites/create', data)
      .subscribe({
        next: data => {
          this.accessService.createSuccessNotification("Site created successfully!")
          this.closeCreateDialog()
        },
        error: error => {
          this.accessService.createErrorNotification(`Failed to Create Site: ${error.message}`)
          this.closeCreateDialog()
        }
      })
  }

  closeCreateDialog() {
    this.dialog.close();
  }
}
