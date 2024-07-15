import { Component, inject, signal } from '@angular/core';
import { UserService } from '../user.service';

@Component({
  selector: 'app-users',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {
  private userService = inject(UserService);

  users = this.userService.users;
  isAddingUser = signal(false);

  onStartAddUser(): void {
    this.isAddingUser.set(true);
  }

  onFinishAddUser(): void {
    this.isAddingUser.set(false);
  }
}
