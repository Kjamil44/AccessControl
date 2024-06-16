import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { NotificationService } from '@progress/kendo-angular-notification';
import { MessageService } from 'primeng/api';
import { map, Observable } from "rxjs";
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccessControlService {

  baseApiUrl: string = environment.baseApiUrl;
  constructor(private http: HttpClient, private notificationService: NotificationService, private messageService: MessageService) { }

  getForDashboard(endpoint: string): Observable<GridDataResult> {
    return this.http
      .get(`${this.baseApiUrl}/api/dashboard/${endpoint}`)
      .pipe(
        map((response: any) => (<GridDataResult>{
          data: response
        })),
      );
  }


  get(url: string): Observable<GridDataResult> {
    return this.http
      .get(`${this.baseApiUrl}/${url}`)
      .pipe(
        map((response: any) => (<GridDataResult>{
          data: response.items
        })),
      );
  }

  getById(url: string, id: string): Observable<GridDataResult> {
    return this.http
      .get(`${this.baseApiUrl}/${url}/${id}`)
      .pipe(
        map((response: any) => (<GridDataResult>{
          data: response
        })),
      );
  }

  create(url: string, request: any): Observable<any> {
    return this.http.post<any>(`${this.baseApiUrl}/${url}`, request);
  }

  update(url: string, id: string, request: any): Observable<any> {
    return this.http.put<any>(`${this.baseApiUrl}/${url}/${id}`, request);
  }

  delete(url: string, id: string): Observable<any> {
    return this.http.delete<any>(`${this.baseApiUrl}/${url}/${id}`);
  }

  public createSuccessNotification(message: any) {
    this.messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: message,
      life: 5000
    });
  }

  public createErrorNotification(message: any) {
    this.messageService.add({
      severity: 'error',
      summary: 'Error',
      detail: message
    });
  }
}
