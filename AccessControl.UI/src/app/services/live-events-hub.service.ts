// live-events-hub.service.ts
import { Injectable, NgZone } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface LiveEventDto {
    liveEventId: string;
    siteId: string;
    entityId: string;
    entityType: string;
    type: number;
    name: string;
    message: string;
    messageType: number;
    dateCreated: string; 
}

@Injectable({ providedIn: 'root' })
export class LiveEventsHubService {
    private connection?: HubConnection;
    private _events$ = new BehaviorSubject<LiveEventDto | null>(null);

    /** Stream of individual events as they arrive */
    get events$(): Observable<LiveEventDto | null> { return this._events$.asObservable(); }

    constructor(private zone: NgZone) { }

    start(): void {
        if (this.connection) return;

        const hubUrl = `${environment.baseApiUrl}/hubs/live-events`;

        this.connection = new HubConnectionBuilder()
            .withUrl(hubUrl)
            .withAutomaticReconnect([0, 2000, 5000, 10000])
            .configureLogging(LogLevel.Information)
            .build();

        this.connection.on('liveEvent', (payload: LiveEventDto) => {
            // run inside Angular zone so UI updates without manual change detection
            this.zone.run(() => this._events$.next(payload));
        });

        this.connection.onclose(err => console.log('[LiveEvents] connection closed', err));
        this.connection.onreconnecting(() => console.log('[LiveEvents] reconnecting...'));
        this.connection.onreconnected(() => console.log('[LiveEvents] reconnected'));

        this.connection
            .start()
            .then(() => console.log('[LiveEvents] connected:', hubUrl))
            .catch(err => console.log('[LiveEvents] start error', err));
    }

    stop(): void {
        this.connection?.stop().catch(() => { /* ignore */ });
        this.connection = undefined;
    }
}
