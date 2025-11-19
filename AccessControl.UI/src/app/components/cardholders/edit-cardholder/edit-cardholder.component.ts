import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-edit-cardholder',
  templateUrl: './edit-cardholder.component.html',
  styleUrls: ['./edit-cardholder.component.css']
})
export class EditCardholderComponent implements OnInit {
  cardholder: any;
  formGroup: FormGroup;

  constructor(private dialogref: DynamicDialogRef,
    private accessService: AccessControlService,
    private config: DynamicDialogConfig) {
    this.formGroup = new FormGroup({
      firstName: new FormControl(),
      lastName: new FormControl(),
      cardNumber: new FormControl(),
    });
  }
  ngOnInit(): void {
    this.cardholder = this.config.data.cardholder;
    this.formGroup.controls['firstName'].setValue(this.cardholder.firstName);
    this.formGroup.controls['lastName'].setValue(this.cardholder.lastName);
    this.formGroup.controls['cardNumber'].setValue(this.cardholder.cardNumber);
  }

  editCardholder() {
    const data = {
      "firstName": this.formGroup.value.firstName,
      "lastName": this.formGroup.value.lastName,
      "cardNumber": this.formGroup.value.cardNumber
    }

    this.accessService.update(`api/cardholders`, this.cardholder.cardholderId, data).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Cardholer edited successfully!")
        this.closeEditDialog()
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message)
        this.closeEditDialog()
      }
    })
  }

  closeEditDialog() {
    this.dialogref.close();
  }
}
