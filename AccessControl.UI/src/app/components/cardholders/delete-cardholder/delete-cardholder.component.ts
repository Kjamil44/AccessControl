import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';
import { SpinnerService } from 'src/app/services/spinner.service';

@Component({
  selector: 'app-delete-cardholder',
  templateUrl: './delete-cardholder.component.html',
  styleUrls: ['./delete-cardholder.component.css'],
})
export class DeleteCardholderComponent implements OnInit {
  cardholder: any;
  siteName: any;

  constructor(
    private dialogref: DynamicDialogRef,
    private config: DynamicDialogConfig,
    public spinner: SpinnerService,
    private accessService: AccessControlService
  ) {}

  ngOnInit(): void {
    this.cardholder = this.config.data.cardholder;
    this.siteName = this.cardholder.siteName;
  }

  deleteCardholder() {
    this.spinner
      .with(
        this.accessService.delete(
          `api/cardholders`,
          this.cardholder.cardholderId
        )
      )
      .subscribe({
        next: (data) => {
          this.accessService.createSuccessNotification(
            'Cardholder deleted successfully!'
          );
          this.closeDeleteDialog();
        },
        error: (err: Error) => {
          this.accessService.createErrorNotification(err.message);
          this.closeDeleteDialog();
        },
      });
  }

  closeDeleteDialog() {
    this.dialogref.close();
  }
}
