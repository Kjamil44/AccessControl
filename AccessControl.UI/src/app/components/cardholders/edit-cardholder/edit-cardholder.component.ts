import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-edit-cardholder',
  templateUrl: './edit-cardholder.component.html',
  styleUrls: ['./edit-cardholder.component.css']
})
export class EditCardholderComponent extends DialogContentBase implements OnInit {
  site: any
  cardholder: any
  firstValue: any
  lastValue: any
  cardValue: any

  constructor(public override dialog: DialogRef, private accessService: AccessControlService) {
    super(dialog);
  }
  ngOnInit(): void {
    this.firstValue = this.cardholder.firstName
    this.lastValue = this.cardholder.lastName
    this.cardValue = this.cardholder.cardNumber
  }

  editCardholder() {
    const data = {
      "firstName": this.firstValue,
      "lastName": this.lastValue,
      "cardNumber": this.cardValue
    }
    this.accessService.update(`api/cardholders/update`, this.cardholder.cardholderId, data).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Cardholer edited successfully!")
        this.closeEditDialog()
      },
      error: error => {
        this.accessService.createErrorNotification(error.message)
        this.closeEditDialog()
      }
    })
  }

  closeEditDialog() {
    this.dialog.close();
  }
}
