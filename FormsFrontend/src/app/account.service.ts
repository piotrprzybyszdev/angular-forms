import { HttpClient } from "@angular/common/http";
import { inject, Injectable, signal } from "@angular/core";
import { Observable, tap } from "rxjs";


export type ApiException = {
  status: number,
  title: string,
  validaionFailures: { Title: string, Details: string }[]
}

const apiRoute = "/api"

@Injectable({
    providedIn: 'root',
})
export class AccountService {
  private httpClient = inject(HttpClient);
  private loggedInAccountId = signal<number | undefined>(undefined);

  loggedInId = this.loggedInAccountId.asReadonly();

  registerAccount(firstName: string, lastName: string, email: string, password: string): Observable<ApiException> {
    return this.httpClient.post<ApiException>(apiRoute + '/register', {
      firstName: firstName,
      lastName: lastName,
      email: email,
      password: password
    });
  }

  logIn(email: string, password: string): Observable<number> {
    return this.httpClient.post<number>(apiRoute + '/login', {
      email: email,
      password: password
    }).pipe(tap({
      next: res => this.loggedInAccountId.set(res)
    }));
  }

  logOut(): Observable<Object> {
    return this.httpClient.post(apiRoute + '/logout', {
      next: () => this.loggedInAccountId.set(undefined)
    });
  }
}
