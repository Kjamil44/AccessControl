import { Component, OnInit } from '@angular/core';
import { AccessControlService } from 'src/app/services/access-control.service';
import { AllowedUsersLockComponent } from '../allowed-users-lock/allowed-users-lock.component';
import { CreateLockComponent } from '../create-lock/create-lock.component';
import { DeleteLockComponent } from '../delete-lock/delete-lock.component';
import { EditLockComponent } from '../edit-lock/edit-lock.component';
import { DialogService } from 'primeng/dynamicdialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-locks-list',
  templateUrl: './locks-list.component.html',
  styleUrls: ['./locks-list.component.css'],
  providers: [DialogService]
})
export class LocksListComponent implements OnInit {
  sites: any[] = []
  locks: any[] = []
  lockIsPresent: boolean = false;
  siteId: any = localStorage.getItem("selectedSiteId"); 
  siteName: any = localStorage.getItem("selectedSiteName"); 
  site: any;


  constructor(private accessService: AccessControlService, private dialog: DialogService, private router: Router) { }

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

  showLocks(site: any) {
    this.siteName = site.displayName; 
    this.siteId = site.siteId;
    this.site = site;
    this.accessService.get(`api/locks/site/${this.siteId}`).subscribe({
      next: (response) => {
        this.locks = response.data;
        this.lockIsPresent = true;
      },
      error: (response) => {
        this.locks = []
        this.lockIsPresent = false;
      }
    })
  }

  onCreate() {
    const ref = this.dialog.open(CreateLockComponent, {
      header: 'Create Lock',
      width: '610px',
      height: '250px',
      baseZIndex: 10000,
      data: {
        siteId: this.siteId
      }
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEdit(lock: any) {
    const ref = this.dialog.open(EditLockComponent, {
      header: `Edit Lock from ${this.siteName}`,
      width: '610px',
      height: '250px',
      baseZIndex: 10000,
      data: {
        lock: lock
      }
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEditAccess(lock: any) {
    this.router.navigate([`/locks/edit-access/${lock.lockId}`]);
  }

  onDelete(lock: any) {
    const ref = this.dialog.open(DeleteLockComponent, {
      header: 'Delete Lock',
      width: '470px',
      height: '250px',
      baseZIndex: 10000,
      data: {
        lock: lock,
        siteName: this.siteName
      }
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

}