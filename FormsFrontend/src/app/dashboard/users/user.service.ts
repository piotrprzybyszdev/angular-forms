import { HttpClient } from "@angular/common/http";
import { Injectable, inject, signal } from "@angular/core";
import { ApiException } from "../../account.service";
import { Observable, map, tap } from "rxjs";

export type User = {
  id: string,
  firstName: string,
  lastName: string,
  email: string,
};

const apiRoute = "/api/user"

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private httpClient = inject(HttpClient);
  private _users = signal<User[]>([]);

  users = this._users.asReadonly();

  constructor() {
    this.fetchUsers();
  }

  addUser(firstName: string, lastName: string, email: string, password: string): Observable<ApiException> {
    return this.httpClient.post<{ result: ApiException }>(apiRoute + '/create', {
      firstName: firstName,
      lastName: lastName,
      email: email,
      password: password
    }, { withCredentials: true }).pipe(tap(res => {
      this.fetchUsers();
    })).pipe(map(res => res.result));
  }

  deleteUser(guid: string): Observable<Object> {
    return this.httpClient.delete(apiRoute + `/delete/${guid}`, { withCredentials: true })
      .pipe(tap(res => {
        this.fetchUsers();
      }));
  }

  updateUser(user: User): Observable<ApiException> {
    return this.httpClient.put<ApiException>(apiRoute + '/update', user, { withCredentials: true })
      .pipe(tap({
          next: res => {
            this.fetchUsers();
          }
        }));
  }

  private fetchUsers(): void {
    this.httpClient.get<User[]>(apiRoute + '/get', { withCredentials: true }).subscribe({
      next: res => {
        this._users.set(res);
      }
    });
  }
}
