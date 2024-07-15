import { effect, inject, Injectable, signal } from "@angular/core";
import { AccountService } from "../../account.service";
import { HttpClient } from "@angular/common/http";
import { Observable, tap } from "rxjs";

export type Task = {
  id: number,
  userGuid: string,
  title: string,
  description: string,
  creationDate: string,
  modificationDate: string,
  dueDate: string
};

const apiRoute = "/api/task"

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private httpClient = inject(HttpClient);
  private accountService = inject(AccountService);
  private _tasks = signal<Task[]>([]);

  tasks = this._tasks.asReadonly();

  constructor() {
    effect(() => this.fetchTasks());
  }

  addTask(title: string, description: string, dueDate: string): Observable<number> {
    return this.httpClient.post<number>(apiRoute + '/create', {
      userGuid: this.accountService.loggedInGuid(),
      title: title,
      description: description,
      dueDate: dueDate
    }, { withCredentials: true }).pipe(tap(res => {
      this.fetchTasks();
    }));
  }

  deleteTask(id: number): Observable<Object> {
    return this.httpClient.delete(apiRoute + `/delete/${id}`, { withCredentials: true })
      .pipe(tap(res => {
        this.fetchTasks();
      }));
  }

  updateTask(task: Task): Observable<Object> {
    return this.httpClient.put(apiRoute + '/update', task, { withCredentials: true })
      .pipe(tap(res => {
        this.fetchTasks();
      }));
  }

  private fetchTasks(): void {
    this.httpClient.get<Task[]>(apiRoute + `/get/${this.accountService.loggedInGuid()}`)
      .subscribe({
        next: res => {
          this._tasks.set(res);
        }
      });
  }
}
