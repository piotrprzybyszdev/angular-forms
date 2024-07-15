import { HttpClient } from "@angular/common/http";
import { inject, Injectable, signal } from "@angular/core";
import { Observable, tap } from "rxjs";

export type Account = {
    guid: string,
    firstName: string,
    lastName: string,
    email: string,
    password: string
};

export type RegistrationResponse = {
  succeeded: boolean,
  errors: { description: string }[]
}

const apiRoute = "/api"

@Injectable({
    providedIn: 'root',
})
export class AccountService {
  private httpClient = inject(HttpClient);
  private loggedInAccountGuid = signal<string | undefined>(undefined);

  loggedInGuid = this.loggedInAccountGuid.asReadonly();

  registerAccount(firstName: string, lastName: string, email: string, password: string): Observable<RegistrationResponse> {
    return this.httpClient.post<RegistrationResponse>(apiRoute + '/register', {
      firstName: firstName,
      lastName: lastName,
      email: email,
      password: password
    });
  }

  logIn(email: string, password: string): Observable<{ guid?: string }> {
    return this.httpClient.post<{guid?: string}>(apiRoute + '/login', {
      email: email,
      password: password
    }).pipe(tap({
      next: res => this.loggedInAccountGuid.set(res.guid)
    }));
  }

  logOut(): Observable<Object> {
    return this.httpClient.post(apiRoute + '/logout', {
      next: () => this.loggedInAccountGuid.set(undefined)
    });
  }
}
