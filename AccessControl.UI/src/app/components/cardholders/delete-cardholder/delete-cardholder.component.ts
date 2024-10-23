import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-delete-cardholder',
  templateUrl: './delete-cardholder.component.html',
  styleUrls: ['./delete-cardholder.component.css']
})
export class DeleteCardholderComponent implements OnInit {
  cardholder: any;
  siteName: any;

  constructor(private dialogref: DynamicDialogRef,
    private config: DynamicDialogConfig,
    private accessService: AccessControlService) {
  }

  ngOnInit(): void {
    this.cardholder = this.config.data.cardholder;
    this.siteName = this.cardholder.siteName;
  }


  deleteCardholder() {
    this.accessService.delete(`api/cardholders/delete`, this.cardholder.cardholderId).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Cardholder deleted successfully!")
        this.closeDeleteDialog()
      },
      error: error => {
        this.accessService.createErrorNotification(error.message)
        this.closeDeleteDialog()
      }
    })
  }

  closeDeleteDialog() {
    this.dialogref.close();
  }
}
