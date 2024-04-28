import { Component, OnInit } from '@angular/core';
import { AccessControlService } from 'src/app/services/access-control.service';
import { CreateSiteComponent } from '../create-site/create-site.component';
import { DeleteSiteComponent } from '../delete-site/delete-site.component';
import { EditSiteComponent } from '../edit-site/edit-site.component';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-sites-list',
  templateUrl: './sites-list.component.html',
  styleUrls: ['./sites-list.component.css'],
  providers: [DialogService]
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
    const ref = this.dialog.open(CreateSiteComponent, {
      header: 'Create Site',
      width: '600px',
      height: '250px',
      baseZIndex: 10000
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onEdit(site: any) {
    const ref = this.dialog.open(EditSiteComponent, {
      header: 'Edit Site',
      width: '600px',
      height: '250px',
      baseZIndex: 10000,
      data: {
        site: site
      }
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  onDelete(site: any) {
    const ref = this.dialog.open(DeleteSiteComponent, {
      header: 'Delete Site',
      width: '450px',
      height: '200px',
      baseZIndex: 10000,
      data: {
        site: site
      }
    });

    ref.onClose.subscribe(() => {
      this.ngOnInit();
    });
  }

  setSiteId(args: any){
    this.selectedSiteId = args.data.siteId;
    this.selectedSiteName = args.data.displayName;
    localStorage.setItem("selectedSiteId",this.selectedSiteId )
    localStorage.setItem("selectedSiteName", this.selectedSiteName)
  }
}
