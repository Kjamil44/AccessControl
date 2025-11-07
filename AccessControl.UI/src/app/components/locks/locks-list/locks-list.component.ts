import { Component, OnInit } from '@angular/core';
import { AccessControlService } from 'src/app/services/access-control.service';
import { CreateLockComponent } from '../create-lock/create-lock.component';
import { DeleteLockComponent } from '../delete-lock/delete-lock.component';
import { EditLockComponent } from '../edit-lock/edit-lock.component';
import { DialogService } from 'primeng/dynamicdialog';
import { Router } from '@angular/router';
import { LockUnlockComponent } from '../lock-unlock/lock-unlock.component';

@Component({
  selector: 'app-locks-list',
  templateUrl: './locks-list.component.html',
  styleUrls: ['./locks-list.component.css'],
  providers: [DialogService]
})
export class LocksListComponent implements OnInit {
  locks: any[] = [];
  lockIsPresent = false;
  siteId: any = localStorage.getItem('selectedSiteId');
  siteName: any = localStorage.getItem('selectedSiteName');
  site: any;

  constructor(
    private accessService: AccessControlService,
    private dialog: DialogService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.accessService.getWithParams(`api/locks`, '').subscribe({
      next: (response) => {
        this.locks = response.data;
        this.lockIsPresent = true;
      },
      error: () => {
        this.lockIsPresent = false;
      }
    });
  }

  showLocks(site: any) {
    this.siteName = site.displayName;
    this.siteId = site.siteId;
    this.site = site;

    const request = this.siteName !== 'All Sites' ? { siteId: this.siteId } : '';

    this.accessService.getWithParams(`api/locks`, request).subscribe({
      next: (response) => {
        this.locks = response.data;
        this.lockIsPresent = true;
      },
      error: () => {
        this.locks = [];
        this.lockIsPresent = false;
      }
    });
  }

  onCreate() {
    const ref = this.dialog.open(CreateLockComponent, {
      header: 'Create Lock',
      width: '610px',
      height: '320px',
      baseZIndex: 10000
    });

    ref.onClose.subscribe(() => this.ngOnInit());
  }

  onEdit(lock: any) {
    const ref = this.dialog.open(EditLockComponent, {
      header: `Edit Lock from ${this.siteName}`,
      width: '610px',
      height: '250px',
      baseZIndex: 10000,
      data: { lock }
    });

    ref.onClose.subscribe(() => this.ngOnInit());
  }

  onEditAccess(lock: any) {
    if (lock.numberOfCardholdersPerSite < 1 || lock.numberOfSchedulesPerSite < 1) {
      this.accessService.createInfoNotification(
        'At least one Cardholder and one Schedule are required to assign access to the Lock.'
      );
    } else {
      this.router.navigate([`/locks/${lock.lockId}/edit-access`]);
    }
  }

  onDelete(lock: any) {
    const adjustableHeight = lock.numberOfAllowedUsers > 0 ? '300px' : '250px';
    const ref = this.dialog.open(DeleteLockComponent, {
      header: 'Delete Lock',
      width: '470px',
      height: adjustableHeight,
      baseZIndex: 10000,
      data: { lock, siteName: this.siteName }
    });

    ref.onClose.subscribe(() => this.ngOnInit());
  }

  onToggleLockUnlock(lock: any) {
    const ref = this.dialog.open(LockUnlockComponent, {
      header: lock.isLocked ? 'Trigger Unlock' : 'Trigger Lock',
      width: '480px',
      height: '450px',
      baseZIndex: 10000,
      data: { lock }
    });

    ref.onClose.subscribe((didChange) => { if (didChange) this.ngOnInit(); });
  }
}