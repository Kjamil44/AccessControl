import { Component, OnInit } from '@angular/core';
import { DialogRef, DialogService } from '@progress/kendo-angular-dialog';
import { AddEvent, EditEvent, RemoveEvent } from '@progress/kendo-angular-grid';
import { AccessControlService } from 'src/app/services/access-control.service';
import { CreateSiteComponent } from '../create-site/create-site.component';
import { DeleteSiteComponent } from '../delete-site/delete-site.component';
import { EditSiteComponent } from '../edit-site/edit-site.component';

@Component({
  selector: 'app-sites-list',
  templateUrl: './sites-list.component.html',
  styleUrls: ['./sites-list.component.css']
})
export class SitesListComponent implements OnInit {
  sites: any[] = [];
  selectedSiteId: any;
  selectedSiteName: any = "None";

  constructor(private accessService: AccessControlService, private dialog: DialogService) { }

  ngOnInit(): void {
    this.accessService.get('api/sites').subscribe({
      next: (response) => {
        this.sites = response.data;
      },
      error: (response) => {
        this.accessService.createErrorNotification("Incorrect api endpoint");
      }
    })
  }

  onCreate() {
    const dialogRef = this.dialog.open({
      content: CreateSiteComponent
    });
    dialogRef.result.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEdit(args: AddEvent) {
    this.accessService.getById('api/sites', args.dataItem.siteId).subscribe({
      next: (response) => {
        dialogRef.content.instance.site = response.data
      },
      error: (response) => {
        console.log(response)
      }
    })
    const dialogRef = this.dialog.open({
      content: EditSiteComponent     
    });
    dialogRef.content.instance.site = args.dataItem;
    dialogRef.result.subscribe(() => {
      this.ngOnInit();
    });
  }

  onDelete(args: RemoveEvent) {
    this.accessService.getById('api/sites', args.dataItem.siteId).subscribe({
      next: (response) => {
        dialogRef.content.instance.site = response.data
      },
      error: (response) => {
        console.log(response)
      }
    })
    const dialogRef = this.dialog.open({
      content: DeleteSiteComponent,     
    });
    dialogRef.result.subscribe(() => {
      this.ngOnInit();
    });
  }

  setSiteId(args: any){
    this.selectedSiteId = args.dataItem.siteId;
    this.selectedSiteName = args.dataItem.displayName;
    localStorage.setItem("selectedSiteId",this.selectedSiteId )
    localStorage.setItem("selectedSiteName", this.selectedSiteName)
  }
}
