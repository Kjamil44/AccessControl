import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-delete-lock',
  templateUrl: './delete-lock.component.html',
  styleUrls: ['./delete-lock.component.css']
})
export class DeleteLockComponent implements OnInit {
  lock: any;
  siteName: any;

  constructor(private dialogref: DynamicDialogRef,
    private config: DynamicDialogConfig,
    private accessService: AccessControlService) {
  }

  ngOnInit(): void {
    this.lock = this.config.data.lock;
    this.siteName = this.lock.siteName;
  }

  deleteLock() {
    this.accessService.delete(`api/locks`, this.lock.lockId).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Lock deleted successfully!")
        this.closeDeleteDialog()
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message)
        this.closeDeleteDialog()
      }
    })
  }

  closeDeleteDialog() {
    this.dialogref.close();
  }

}
