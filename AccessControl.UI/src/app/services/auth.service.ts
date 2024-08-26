import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { map, Observable } from "rxjs";
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    baseApiUrl: string = environment.baseApiUrl;

    constructor(private http: HttpClient) { }

    login(credentials: any):
        Observable<any> {
        return this.http.post<any>(`${this.baseApiUrl}/auth/login`,
            credentials);
    }

    register(credentials: any): Observable<any> {
        return this.http.post<any>(`${this.baseApiUrl}/auth/register`, credentials);
    }

    getToken(): string | null {
        return localStorage.getItem('token');
    }

    storeToken(token: string) {
        localStorage.setItem('token', token);
    }

    removeToken() {
        localStorage.removeItem('token');
    }
}