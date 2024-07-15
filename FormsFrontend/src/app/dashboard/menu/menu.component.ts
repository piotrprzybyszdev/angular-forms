import { Component, inject } from '@angular/core';
import { AccountService } from '../../account.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {
  private accountService = inject(AccountService);

  logOut(): void {
    this.accountService.logOut().subscribe();
  }
}
