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

    this.accessService.update('api/sites', this.site.siteId, data).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Site edited successfully!")
        this.closeCreateDialog()
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message)
        this.closeCreateDialog()
      }
    })
  }

  closeCreateDialog() {
    this.dialogref.close();
  }

}
