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
        this.sites = response.data;
      },
      error: (response) => {
        this.accessService.createErrorNotification("Incorrect api endpoint");
      }
    })
  }

  setSiteId(args: any){
    this.site.emit(args.value);
    localStorage.setItem("selectedSiteId",args.value.siteId);
    localStorage.setItem("selectedSiteName", this.selectedSite.displayName);
  }
}
