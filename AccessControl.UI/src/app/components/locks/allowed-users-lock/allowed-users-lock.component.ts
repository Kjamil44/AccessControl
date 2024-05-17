import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AccessControlService } from 'src/app/services/access-control.service';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-allowed-users-lock',
  templateUrl: './allowed-users-lock.component.html',
  styleUrls: ['./allowed-users-lock.component.css']
})
export class AllowedUsersLockComponent implements OnInit {
  site: any;
  lock: any;

  users: any[] = []
  cardholders: any[] = []
  schedules: any[] = []

  userIsPresent: boolean = false;
  show: boolean = false;
  remove: boolean = false;
  edit: boolean = false;

  selectedValue: any
  editCardholder: any
  clicked = false;
  formGroup: FormGroup;

  constructor(private dialogref: DynamicDialogRef,
    private accessService: AccessControlService,
    private config: DynamicDialogConfig) {
    this.formGroup = new FormGroup({
      cardholderId: new FormControl(),
      scheduleId: new FormControl(),
    })
  }

  ngOnInit(): void {
    this.accessService.getById('api/sites', this.config.data.siteId).subscribe({
      next: (response) => {
        this.site = response.data;
        this.lock = this.config.data.lock;

        this.users = this.lock.assignedUsers;
        this.users.forEach(element => {
          if (element.cardholderName == null || element.scheduleName == null) {
            this.userIsPresent = false;
          }
          else {
            this.userIsPresent = true;
          }
        });
      },
      error: (response) => {
        this.accessService.createErrorNotification("Incorrect api endpoint");
      }
    })
   
  
  }

  ChooseNewUser() {
    this.accessService.get(`api/cardholders/site/${this.site.siteId}`).subscribe({
      next: (response) => {
        this.cardholders = response.data;
      },
      error: (response) => {
        this.accessService.createErrorNotification("No Cardholders found, please Add Cardholder for Lock!")
      }
    })
    this.accessService.get(`api/schedules/site/${this.site.siteId}`).subscribe({
      next: (response) => {
        this.schedules = response.data;
      },
      error: (response) => {
        this.accessService.createErrorNotification("No Schedules found, please Add Schedule for Lock!")
      }
    })
  }

  EditUser() {
    this.accessService.get(`api/schedules/site/${this.site.siteId}`).subscribe({
      next: (response) => {
        this.schedules = response.data;
      },
      error: (response) => {
        this.accessService.createErrorNotification(response.message)
      }
    })
  }

  AssignUserToLock() {
    const data = {
      "cardholderId": this.formGroup.value.cardholderId,
      "scheduleId": this.formGroup.value.scheduleId
    }

    this.accessService.create(`api/locks/${this.lock.lockId}/assign`, data)
      .subscribe({
        next: data => {
          this.accessService.createSuccessNotification("User assigned successfully!")
          this.closeDialog();
        },
        error: error => {
          this.accessService.createErrorNotification(error.message)
          this.closeDialog();
        }
      })
  }

  EditUserToLock(cardholderId: any) {
    const data = {
      "scheduleId": this.formGroup.value.scheduleId
    }
    this.accessService.update(`api/lockS/${this.lock.lockId}/edit`, cardholderId, data)
      .subscribe({
        next: data => {
          this.accessService.createSuccessNotification("Allowed User's Schedule updated successfully!")
          this.closeDialog();
        },
        error: error => {
          this.accessService.createErrorNotification(error.message)
          this.closeDialog();
        }
      })
  }

  RemoveUserFromLock(cardholderId: any) {
    this.accessService.delete(`api/locks/${this.lock.lockId}/remove`, cardholderId).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("Allowed User removed successfully!")
        this.closeDialog();
      },
      error: error => {
        this.accessService.createErrorNotification(error.message)
        this.closeDialog();
      }
    })
    this.closeDialog();
    window.location.reload()
  }

  closeDialog() {
    this.dialogref.close();
  }
}
