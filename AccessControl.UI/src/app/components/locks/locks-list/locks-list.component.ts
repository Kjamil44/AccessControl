import { Component, OnInit } from '@angular/core';
import { DialogService } from '@progress/kendo-angular-dialog';
import { AddEvent, RemoveEvent } from '@progress/kendo-angular-grid';
import { AccessControlService } from 'src/app/services/access-control.service';
import { AllowedUsersLockComponent } from '../allowed-users-lock/allowed-users-lock.component';
import { CreateLockComponent } from '../create-lock/create-lock.component';
import { DeleteLockComponent } from '../delete-lock/delete-lock.component';
import { EditLockComponent } from '../edit-lock/edit-lock.component';

@Component({
  selector: 'app-locks-list',
  templateUrl: './locks-list.component.html',
  styleUrls: ['./locks-list.component.css']
})
export class LocksListComponent implements OnInit {
  sites: any[] = []
  locks: any[] = []
  lockIsPresent: boolean = false;
  siteId: any = localStorage.getItem("selectedSiteId") 
  siteName: any = localStorage.getItem("selectedSiteName") 


  constructor(private accessService: AccessControlService, private dialog: DialogService) { }

  ngOnInit(): void {
    this.accessService.get('api/sites').subscribe({
      next: (response) => {
        this.sites = response.data;
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message)
      }
    })

    this.accessService.get(`api/locks/site/${this.siteId}`).subscribe({
      next: (response) => {
        this.locks = response.data;
        this.lockIsPresent = true;
      },
      error: (response) => {
        this.lockIsPresent = false;
      }
    })
  }

  showLocks(siteId: any, siteName: any) {
    localStorage.setItem("selectedSiteName",siteName);
    localStorage.setItem("selectedSiteId",siteId);
    this.siteName = siteName 
    this.siteId = siteId;
    this.accessService.get(`api/locks/site/${siteId}`).subscribe({
      next: (response) => {
        this.locks = response.data;
        this.lockIsPresent = true;
      },
      error: (response) => {
        this.lockIsPresent = false;
      }
    })
  }

  onCreate() {
    const dialogRef = this.dialog.open({
      content: CreateLockComponent
    });
    dialogRef.content.instance.siteId = this.siteId;
    dialogRef.result.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEdit(args: AddEvent) {
    this.accessService.getById('api/sites', this.siteId).subscribe({
      next: (response) => {
        dialogRef.content.instance.site = response.data
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message)
      }
    })
    const dialogRef = this.dialog.open({
      content: EditLockComponent
    });
    dialogRef.content.instance.lock = args.dataItem;
    dialogRef.result.subscribe(() => {
      this.ngOnInit();
    });
  }

  onDelete(args: RemoveEvent) {
    this.accessService.getById('api/sites', this.siteId).subscribe({
      next: (response) => {
        dialogRef.content.instance.site = response.data
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message)
      }
    })
    const dialogRef = this.dialog.open({
      content: DeleteLockComponent,
    });
    dialogRef.content.instance.lock = args.dataItem;
    dialogRef.result.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEditAccess(lock: any) {
    this.accessService.getById('api/sites', this.siteId).subscribe({
      next: (response) => {
        dialogRef.content.instance.site = response.data
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message)
      }
    })
    const dialogRef = this.dialog.open({
      content: AllowedUsersLockComponent
    });
    dialogRef.content.instance.lock = lock
    dialogRef.result.subscribe(() => {
      this.ngOnInit();
    });
  }
}