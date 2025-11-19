import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-create-site',
  templateUrl: './create-site.component.html',
  styleUrls: ['./create-site.component.css']
})
export class CreateSiteComponent implements OnInit {
  displayCreateDialog: boolean = false;
  formGroup: FormGroup;

  constructor(private fb: FormBuilder, private accessService: AccessControlService, private dialogref: DynamicDialogRef) {
    this.formGroup = this.fb.group({
      displayName: ['']
    });
  }

  ngOnInit(): void {
  }

  createSite() {
    const data = {
      "displayName": this.formGroup.value.displayName,
    }

    this.accessService.create('api/sites', data)
      .subscribe({
        next: data => {
          this.accessService.createSuccessNotification("Site created successfully!")
          this.closeCreateDialog();
        },
        error: (err: Error) => {
          this.accessService.createErrorNotification(`Failed to Create Site: ${err.message}`);
          this.closeCreateDialog();
        }
      })
  }

  closeCreateDialog() {
    this.dialogref.close();
  }
}
