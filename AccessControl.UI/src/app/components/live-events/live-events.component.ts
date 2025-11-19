// live-events.component.ts
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import {
  LiveEventsHubService,
  LiveEventDto,
} from '../../services/live-events-hub.service';
import { Subject } from 'rxjs';
import { filter, finalize, takeUntil } from 'rxjs/operators';
import { Table, TableModule } from 'primeng/table';
import { AccessControlService } from 'src/app/services/access-control.service';

type MT = LiveEventDto['messageType'];
type LT = LiveEventDto['type'];

@Component({
  selector: 'app-live-events',
  templateUrl: './live-events.component.html',
  styleUrls: ['./live-events.component.css'],
})
export class LiveEventsComponent implements OnInit, OnDestroy {
  @ViewChild('tbl') tbl!: Table;

  rows: LiveEventDto[] = [];
  loading = false;
  maxRows = 500;

  private destroy$ = new Subject<void>();
  private hubSubscribed = false;
  initialized = false;

  // dropdown options
  messageTypeOptions = [
    { label: 'Unknown', value: 0 },
    { label: 'LockTriggerGranted', value: 1 },
    { label: 'LockTriggerDenied', value: 2 },
    { label: 'UnlockTriggerGranted', value: 3 },
    { label: 'UnlockTriggerDenied', value: 4 },
    { label: 'SiteCreated', value: 5 },
    { label: 'SiteDeleted', value: 6 },
    { label: 'SiteUpdated', value: 7 },
    { label: 'LockCreated', value: 8 },
    { label: 'LockDeleted', value: 9 },
    { label: 'LockUpdated', value: 10 },
    { label: 'LockAccessListUpdated', value: 11 },
    { label: 'CardholderCreated', value: 12 },
    { label: 'CardholderDeleted', value: 13 },
    { label: 'CardholderUpdated', value: 14 },
    { label: 'ScheduleCreated', value: 15 },
    { label: 'ScheduleDeleted', value: 16 },
    { label: 'ScheduleUpdated', value: 17 },
  ];

  typeOptions = [
    { label: 'None', value: 0 },
    { label: 'LockUnlock', value: 1 },
    { label: 'SiteOperations', value: 2 },
    { label: 'LockOperations', value: 3 },
    { label: 'CardholderOperations', value: 4 },
    { label: 'ScheduleOperations', value: 5 },
  ];

  entityTypeOptions = [
    { label: 'Site', value: 'Site' },
    { label: 'Lock', value: 'Lock' },
    { label: 'Cardholder', value: 'Cardholder' },
    { label: 'Schedule', value: 'Schedule' },
  ];

  constructor(
    private accessService: AccessControlService,
    private hub: LiveEventsHubService
  ) {}

  ngOnInit(): void {
    this.loadInitial(); // initial GET, then connect hub
  }

  /** Call this from a parent TabView on activate if the component stays alive across tab switches */
  onTabActivated(): void {
    if (!this.initialized) this.loadInitial();
  }

  private loadInitial(take: number = 200): void {
    this.loading = true;

    this.accessService
      .get('api/live-events')
      .pipe(
        finalize(() => {
          this.loading = false;
          this.initialized = true;
        })
      )
      .subscribe({
        next: (res) => {
          const list: LiveEventDto[] = res?.data ?? res ?? [];
          const simpleType = (s: string) => (s ?? '').split('.').pop()!.trim();

          this.rows = [...list]
            .map((e: any) => ({
              ...e,
              dateCreated: new Date(e.dateCreated),
              entityType: simpleType(e.entityType),
            }))
            .sort(
              (a, b) =>
                new Date(b.dateCreated).getTime() -
                new Date(a.dateCreated).getTime()
            )
            .slice(0, this.maxRows);

          this.startHubIfNeeded();
        },
        error: (err: Error) => {
          this.accessService.createErrorNotification(err.message);
          console.log('[LiveEvents] initial load failed', err);
          this.startHubIfNeeded();
        },
      });
  }

  private startHubIfNeeded(): void {
    this.hub.start();

    if (!this.hubSubscribed) {
      this.hubSubscribed = true;
      this.hub.events$
        .pipe(
          takeUntil(this.destroy$),
          filter((e): e is LiveEventDto => !!e)
        )
        .subscribe((e: any) => {
          const normalized = { ...e, dateCreated: new Date(e.dateCreated) };
          this.rows = [normalized, ...this.rows].slice(0, this.maxRows);

          if (this.tbl) {
            this.tbl.sortField = 'dateCreated';
            this.tbl.sortOrder = -1;
            this.tbl.sortSingle();
          }
        });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    // Optional: this.hub.stop();  // keep connected if others use the hub service
  }

  labelOfMessageType(v: MT) {
    return this.messageTypeOptions.find((x) => x.value === v)?.label ?? v;
  }

  labelOfType(v: LT) {
    return this.typeOptions.find((x) => x.value === v)?.label ?? v;
  }

  applyTextFilter(event: Event, field: keyof LiveEventDto) {
    const val = (event.target as HTMLInputElement).value;
    this.tbl.filter(val, field as string, 'contains');
  }

  clearAllFilters() {
    this.tbl.clear();
  }
}
