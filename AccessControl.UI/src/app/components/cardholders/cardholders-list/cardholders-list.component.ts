import { Component, OnInit } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';
import { CreateCardholderComponent } from '../create-cardholder/create-cardholder.component';
import { DeleteCardholderComponent } from '../delete-cardholder/delete-cardholder.component';
import { EditCardholderComponent } from '../edit-cardholder/edit-cardholder.component';
import { SpinnerService } from 'src/app/services/spinner.service';

@Component({
  selector: 'app-cardholders-list',
  templateUrl: './cardholders-list.component.html',
  styleUrls: ['./cardholders-list.component.css'],
  providers: [DialogService],
})
export class CardholdersListComponent implements OnInit {
  cardholders: any[] = [];
  cardholderIsPresent: boolean = false;
  siteId: any = localStorage.getItem('selectedSiteId');
  siteName: any = localStorage.getItem('selectedSiteName');

  constructor(
    private accessService: AccessControlService,
    public spinner: SpinnerService,
    private dialog: DialogService
  ) {}

  ngOnInit(): void {
    this.spinner.show();
    this.accessService.getWithParams(`api/cardholders`, '').subscribe({
      next: (response) => {
        this.cardholders = response.data;
        this.cardholderIsPresent = true;
        this.spinner.hide();
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
        this.cardholderIsPresent = false;
        this.spinner.hide();
      },
    });
  }

  showCardholders(site: any) {
    this.siteName = site.displayName;
    this.siteId = site.siteId;

    let request =
      this.siteName !== 'All Sites'
        ? {
            siteId: this.siteId,
          }
        : '';

    this.accessService.getWithParams(`api/cardholders`, request).subscribe({
      next: (response) => {
        this.cardholders = response.data;
        this.cardholderIsPresent = true;
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
        this.cardholderIsPresent = false;
      },
    });
  }

  onCreate() {
    const ref = this.dialog.open(CreateCardholderComponent, {
      header: 'Create Carholder',
      width: '610px',
      height: '500px',
      baseZIndex: 10000,
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEdit(cardholder: any) {
    const ref = this.dialog.open(EditCardholderComponent, {
      header: `Edit Cardholder from ${cardholder.siteName}`,
      width: '610px',
      height: '440px',
      baseZIndex: 10000,
      data: {
        cardholder: cardholder,
      },
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onDelete(cardholder: any) {
    const ref = this.dialog.open(DeleteCardholderComponent, {
      header: 'Delete Cardholder',
      width: '470px',
      height: '250px',
      baseZIndex: 10000,
      data: {
        cardholder: cardholder,
        siteName: cardholder.siteName,
      },
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }
}
