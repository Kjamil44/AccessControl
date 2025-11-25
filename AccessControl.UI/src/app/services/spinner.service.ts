import { Injectable } from '@angular/core';
import { BehaviorSubject, finalize, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SpinnerService {
  private readonly _visible$ = new BehaviorSubject<boolean>(false);
  readonly visible$ = this._visible$.asObservable();

  show() { this._visible$.next(true); }
  hide() { this._visible$.next(false); }

  with<T>(obs$: Observable<T>): Observable<T> {
    this.show();
    return obs$.pipe(finalize(() => this.hide()));
  }
}
