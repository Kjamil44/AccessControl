import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-create-lock',
  templateUrl: './create-lock.component.html',
  styleUrls: ['./create-lock.component.css']
})
export class CreateLockComponent implements OnInit {
  sites: any[] = [];
  siteId: any
  formGroup: FormGroup;

  constructor(private accessService: AccessControlService,
    private dialogref: DynamicDialogRef,
    private config: DynamicDialogConfig) {
    this.formGroup = new FormGroup({
      displayName: new FormControl(),
      site: new FormControl()
    });
  }

  ngOnInit(): void {
    this.accessService.get('api/sites').subscribe({
      next: (response) => {
        this.sites = response.data;
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message)
      }
    })
  }

  createLock() {
    const data = {
      "siteId": this.formGroup.value.site.siteId,
      "displayName": this.formGroup.value.displayName,
    }

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
    this.dialogref.close();
  }
}
