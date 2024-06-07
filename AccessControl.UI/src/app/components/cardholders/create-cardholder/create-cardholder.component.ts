import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-create-cardholder',
  templateUrl: './create-cardholder.component.html',
  styleUrls: ['./create-cardholder.component.css']
})
export class CreateCardholderComponent implements OnInit  {

  siteId: any
  formGroup: FormGroup;

  constructor(private accessService: AccessControlService,
    private dialogref: DynamicDialogRef,
    private config: DynamicDialogConfig) {
    this.formGroup = new FormGroup({
      firstName: new FormControl(),
      lastName: new FormControl(),
      cardNumber: new FormControl()
    });
  }

  ngOnInit(): void {
    this.siteId = this.config.data.siteId;
  }

  createCardholder() {
    const data = {
      "siteId": this.siteId,
      "firstName": this.formGroup.value.firstName,
      "lastName": this.formGroup.value.lastName,
      "cardNumber": this.formGroup.value.cardNumber
    }
    this.accessService.create(`api/cardholders/create`, data)
      .subscribe({
        next: data => {
          this.accessService.createSuccessNotification("Cardholder created successfully!")
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
