import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'site-selector',
  templateUrl: './site-selector.component.html',
  styleUrl: './site-selector.component.css',
})
export class SiteSelectorComponent implements OnInit {
  sites: any[] = [];
  selectedSite: any;
  @Output() site = new EventEmitter<any>();

  constructor(private accessService: AccessControlService) {}

  ngOnInit(): void {
    this.accessService.get('api/sites').subscribe({
      next: (response) => {
        this.sites = [
          { siteId: null, displayName: 'All Sites' }, // This is the "For All Sites" option
          ...response.data,
        ];

        this.selectedSite = this.sites[0];
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
      },
    });
  }

  setSiteId(args: any) {
    const site = args?.value ?? this.sites[0];
    this.selectedSite = site;
    this.emitAndPersist(site);
  }

  clearSelectedSite() {
    this.selectedSite = this.sites[0];
    this.emitAndPersist(this.selectedSite);
  }

  private emitAndPersist(site: any) {
    this.site.emit(site);
    localStorage.setItem('selectedSiteId', site.siteId ?? '');
    localStorage.setItem('selectedSiteName', site.displayName ?? 'All Sites');
  }
}
