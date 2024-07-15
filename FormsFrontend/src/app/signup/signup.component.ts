import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { AccountService } from '../account.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  password: string = '';

  private accountService = inject(AccountService);
  private router = inject(Router);
  private toastrService = inject(ToastrService);

  onSubmit(): void {
    this.accountService.registerAccount(
      this.firstName, this.lastName, this.email, this.password
    ).subscribe({
      next: res => {
        if (res.succeeded) {
          this.router.navigate(['login']);
          this.toastrService.success('You have been successfully signed up!', 'Sign up');
        } else {
          this.toastrService.error(res.errors[0].description, 'Sign up');
        }
      }
    });
  }
}
