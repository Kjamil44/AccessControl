import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Location } from '@angular/common';
import { AccessControlService } from 'src/app/services/access-control.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-allowed-users-lock',
  templateUrl: './allowed-users-lock.component.html',
  styleUrls: ['./allowed-users-lock.component.css']
})
export class AllowedUsersLockComponent implements OnInit {
  siteId: any;
  siteDisplayName: any;
  lock: any;

  users: any[] = [];
  cardholders: any[] = [];
  schedules: any[] = [];

  userIsPresent: boolean = false;
  show: boolean = false;
  remove: boolean = false;
  edit: boolean = false;

  selectedValue: any;
  editCardholder: any;
  clicked = false;
  formGroup: FormGroup;

  constructor(private accessService: AccessControlService, private route: ActivatedRoute, private location: Location) {
    this.formGroup = new FormGroup({
      cardholder: new FormControl(),
      schedule: new FormControl(),
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      let lockId = params['id'];

      this.accessService.getById('api/locks', lockId).subscribe({
        next: (response) => {
          this.lock = response.data;
          this.siteId = this.lock.siteId;
          this.siteDisplayName = this.lock.siteDisplayName;

          let request = {
            siteId: this.siteId
          };

          this.accessService.getWithParams(`api/schedules`, request).subscribe({
            next: (response) => {
              this.schedules = response.data;

              this.users = this.lock.assignedUsers.map((user: any) => ({
                ...user,
                editing: false,
                scheduleControl: new FormControl(this.schedules.filter(x => x.scheduleId === user.scheduleId)[0])
              }));

              this.userIsPresent = this.users.length > 0;
            },
            error: (err: Error) => {
              this.accessService.createErrorNotification(err.message);
            }
          });
          
          this.accessService.getWithParams(`api/cardholders`, request).subscribe({
            next: (response) => {
              this.cardholders = response.data;
              
            },
            error: (err: Error) => {
              this.accessService.createErrorNotification(err.message);
            }
          });
        },
        error: (err: Error) => {
          this.accessService.createErrorNotification(err.message);
        }
      });
    });
  }

  chooseNewUser() {
    this.show = true;
  }

  editUser(user: any) {
    user.editing = true;
  }

  cancelEdit(user: any) {
    user.editing = false;
  }

  saveEdit(user: any) {
    const allowedUser = user.scheduleControl.value;

    this.accessService.update(`api/locks/${this.lock.lockId}/edit`, user.cardholderId, allowedUser).subscribe({
      next: data => {
        user.scheduleName = this.schedules.find(schedule => schedule.scheduleId === data.scheduleId)?.displayName;
        user.editing = false;
        this.accessService.createSuccessNotification("Allowed User's Schedule updated successfully!");
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
      }
    });
  }

  removeUser(user: any, event: Event) {
    event.stopPropagation(); // Prevents triggering the edit on click
    this.accessService.delete(`api/locks/${this.lock.lockId}/remove`, user.cardholderId).subscribe({
      next: data => {
        this.users = this.users.filter(u => u.cardholderId !== user.cardholderId);
        this.userIsPresent = this.users.length > 0;
        this.accessService.createSuccessNotification("Allowed User removed successfully!");
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
      }
    });
  }

  assignUserToLock() {
    const data = {
      cardholderId: this.formGroup.value.cardholder.cardholderId,
      scheduleId: this.formGroup.value.schedule.scheduleId
    };

    this.accessService.create(`api/locks/${this.lock.lockId}/assign`, data).subscribe({
      next: data => {
        this.accessService.createSuccessNotification("User assigned successfully!");
        this.closeDialog();
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
        this.closeDialog();
      }
    });
  }

  closeDialog() {
    this.location.back();
  }

  isUserSelected(user: any): boolean {
    return user.editing;
  }

  toggleUserActions(user: any) {
    user.editing = !user.editing;
  }
}
