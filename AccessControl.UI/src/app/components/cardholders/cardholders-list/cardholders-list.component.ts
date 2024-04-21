import { Component, OnInit } from '@angular/core';
import { DialogService } from '@progress/kendo-angular-dialog';
import { AddEvent, RemoveEvent } from '@progress/kendo-angular-grid';
import { AccessControlService } from 'src/app/services/access-control.service';
import { CreateCardholderComponent } from '../create-cardholder/create-cardholder.component';
import { DeleteCardholderComponent } from '../delete-cardholder/delete-cardholder.component';
import { EditCardholderComponent } from '../edit-cardholder/edit-cardholder.component';

@Component({
  selector: 'app-cardholders-list',
  templateUrl: './cardholders-list.component.html',
  styleUrls: ['./cardholders-list.component.css']
})
export class CardholdersListComponent implements OnInit {
  sites: any[] = []
  cardholders: any[] = []
  cardholderIsPresent: boolean = false;
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

    this.accessService.get(`api/cardholders/site/${this.siteId}`).subscribe({
      next: (response) => {
        this.cardholders = response.data;
        this.cardholderIsPresent = true;
      },
      error: (response) => {
        this.cardholderIsPresent = false;
      }
    })
  }

  showCardholders(siteId: any,siteName: any) {
    localStorage.setItem("selectedSiteName",siteName);
    localStorage.setItem("selectedSiteId",siteId);
    this.siteName = siteName 
    this.siteId = siteId;
    this.accessService.get(`api/cardholders/site/${siteId}`).subscribe({
      next: (response) => {
        this.cardholders = response.data;
        this.cardholderIsPresent = true;
      },
      error: (response) => {
        this.cardholderIsPresent = false;
      }
    })
  }

  onCreate() {
    const dialogRef = this.dialog.open({
      content: CreateCardholderComponent
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
        console.log(response)
      }
    })
    const dialogRef = this.dialog.open({
      content: EditCardholderComponent
    });
    dialogRef.content.instance.cardholder = args.dataItem;
    dialogRef.content.instance.cardValue = args.dataItem.cardNumber
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
      content: DeleteCardholderComponent,
    });
    dialogRef.content.instance.cardholder = args.dataItem;
    dialogRef.result.subscribe(() => {
      this.ngOnInit();
    });
  }

}
