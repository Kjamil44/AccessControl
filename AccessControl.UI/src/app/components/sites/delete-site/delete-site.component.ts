import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { SpinnerService } from 'src/app/services/spinner.service';

@Component({
  selector: 'app-delete-site',
  templateUrl: './delete-site.component.html',
  styleUrls: ['./delete-site.component.css'],
  providers: [ConfirmationService, MessageService],
})
export class DeleteSiteComponent implements OnInit {
  site: any;

  constructor(
    private dialogref: DynamicDialogRef,
    private config: DynamicDialogConfig,
    public spinner: SpinnerService,
    private accessService: AccessControlService
  ) {}

  ngOnInit(): void {
    this.site = this.config.data.site;
  }

  deleteSite() {
    this.spinner
      .with(this.accessService.delete('api/sites', this.site.siteId))
      .subscribe({
        next: (data) => {
          this.accessService.createSuccessNotification(
            'Site deleted successfully!'
          );
          this.closeDeleteDialog();
        },
        error: (err: Error) => {
          this.accessService.createErrorNotification(err);
          this.closeDeleteDialog();
        },
      });
  }

  closeDeleteDialog() {
    this.dialogref.close();
  }
}
