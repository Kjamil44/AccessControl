import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-delete-site',
  templateUrl: './delete-site.component.html',
  styleUrls: ['./delete-site.component.css'],
  providers: [ConfirmationService, MessageService]
})
export class DeleteSiteComponent implements OnInit{

  site: any;

  constructor(private dialogref: DynamicDialogRef, 
    private config: DynamicDialogConfig, 
    private accessService: AccessControlService) {
  }

  ngOnInit(): void {
      this.site = this.config.data.site;
  }

  deleteSite() {
    this.accessService.delete('api/sites/delete', this.site.siteId).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Site deleted successfully!")
        this.closeDeleteDialog()
      },
      error: error => {
        this.accessService.createErrorNotification("Failed to delete Site: Lock, Cardholder or Schedule still exist on Site!")
        this.closeDeleteDialog()
      }
    })
  }

  closeDeleteDialog() {
    this.dialogref.close();
  }

}
