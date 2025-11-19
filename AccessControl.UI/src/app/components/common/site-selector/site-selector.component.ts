import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'site-selector',
  templateUrl: './site-selector.component.html',
  styleUrl: './site-selector.component.css'
})
export class SiteSelectorComponent  implements OnInit {
  sites: any[] = [];
  selectedSite: any;
  @Output() site = new EventEmitter<any>();

  constructor(private accessService: AccessControlService) { }

  ngOnInit(): void {
    this.accessService.get('api/sites').subscribe({
      next: (response) => {
        this.sites = [
          { siteId: null, displayName: 'All Sites' },  // This is the "For All Sites" option
          ...response.data
        ];

        this.selectedSite = { siteId: null, displayName: 'All Sites' };
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
      }
    })
  }

  setSiteId(args: any){
    this.site.emit(args.value);
    localStorage.setItem("selectedSiteId",args.value.siteId);
    localStorage.setItem("selectedSiteName", this.selectedSite.displayName);
  }
}
