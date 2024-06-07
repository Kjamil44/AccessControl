import { Component, OnInit } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';
import { CreateCardholderComponent } from '../create-cardholder/create-cardholder.component';
import { DeleteCardholderComponent } from '../delete-cardholder/delete-cardholder.component';
import { EditCardholderComponent } from '../edit-cardholder/edit-cardholder.component';

@Component({
  selector: 'app-cardholders-list',
  templateUrl: './cardholders-list.component.html',
  styleUrls: ['./cardholders-list.component.css'],
  providers: [DialogService]
})
export class CardholdersListComponent implements OnInit {
  sites: any[] = []
  cardholders: any[] = []
  cardholderIsPresent: boolean = false;
  siteId: any = localStorage.getItem("selectedSiteId") 
  siteName: any = localStorage.getItem("selectedSiteName") 

  constructor(private accessService: AccessControlService,private dialog: DialogService) { }

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

  showCardholders(site: any) {
    this.siteName = site.displayName; 
    this.siteId = site.siteId;

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

  onCreate() {
    const ref = this.dialog.open(CreateCardholderComponent, {
      header: 'Create Carholder',
      width: '610px',
      height: '400px',
      baseZIndex: 10000,
      data: {
        siteId: this.siteId
      }
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEdit(cardholder: any) {
    // this.accessService.getById('api/sites', this.siteId).subscribe({
    //   next: (response) => {
    //     dialogRef.content.instance.site = response.data
    //   },
    //   error: (response) => {
    //     console.log(response)
    //   }
    // })
    // const dialogRef = this.dialog.open({
    //   content: EditCardholderComponent
    // });
    // dialogRef.content.instance.cardholder = args.dataItem;
    // dialogRef.content.instance.cardValue = args.dataItem.cardNumber
    // dialogRef.result.subscribe(() => {
    //   this.ngOnInit();
    // });
  }

  onDelete(cardholder: any) {
    // this.accessService.getById('api/sites', this.siteId).subscribe({
    //   next: (response) => {
    //     dialogRef.content.instance.site = response.data
    //   },
    //   error: (response) => {
    //     this.accessService.createErrorNotification(response.message)
    //   }
    // })
    // const dialogRef = this.dialog.open({
    //   content: DeleteCardholderComponent,
    // });
    // dialogRef.content.instance.cardholder = args.dataItem;
    // dialogRef.result.subscribe(() => {
    //   this.ngOnInit();
    // });
  }

}
