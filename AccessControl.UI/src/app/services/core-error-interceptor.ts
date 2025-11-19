import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable()
export class CoreErrorInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => {
        let detail = 'An unexpected error occurred.';
        const body = err.error;

        if (body && typeof body === 'object') {
          detail = body.errorMessage || body.ErrorMessage || detail;
        } else if (typeof body === 'string' && body.trim()) {
          detail = body;
        }

        return throwError(() => new Error(detail));
      })
    );
  }
}
