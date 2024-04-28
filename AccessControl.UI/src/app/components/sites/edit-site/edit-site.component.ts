import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-edit-site',
  templateUrl: './edit-site.component.html',
  styleUrls: ['./edit-site.component.css']
})
export class EditSiteComponent implements OnInit {
  site: any;
  submitted = false;
  nameValue: any
  formGroup: FormGroup;

  constructor(private fb: FormBuilder,
    private dialogref: DynamicDialogRef,
    private accessService: AccessControlService,
    private config: DynamicDialogConfig) {
    this.formGroup = this.fb.group({
      displayName: ['']
    });
  }

  ngOnInit(): void {
    this.site = this.config.data.site;
    this.formGroup.controls['displayName'].setValue(this.site.displayName);
  }

  editSite() {
    const data = {
      "displayName": this.formGroup.value.displayName
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
    this.dialogref.close();
  }

}
