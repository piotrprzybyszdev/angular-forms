import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  form!: FormGroup<{
    email: FormControl,
    password: FormControl
  }>;

  get emailInvalid(): boolean {
    return (this.isSubmitted ||
      (this.form.controls.email.touched &&
      this.form.controls.email.dirty)) &&
      this.form.controls.email.invalid;
  }

  get passwordInvalid(): boolean {
    return (this.isSubmitted ||
      (this.form.controls.password.touched &&
      this.form.controls.password.dirty)) &&
      this.form.controls.password.invalid;
  }

  private accountService = inject(AccountService);
  private toastrService = inject(ToastrService);
  private router = inject(Router);
  private isSubmitted = false;

  constructor() {
    this.initForm();
  }

  initForm() {
    this.form = new FormGroup({
      email: new FormControl('', {
        validators: [Validators.email, Validators.required, Validators.maxLength(100)]
      }),
      password: new FormControl('', {
        validators: [Validators.required, Validators.maxLength(100)]
      })
    });
  }

  onSubmit(): void {
    this.isSubmitted = true;

    if (this.form.invalid) {
      return;
    }

    const enteredEmail = this.form.value.email!;
    const enteredPassword = this.form.value.password!;

    this.accountService.logIn(enteredEmail, enteredPassword).subscribe({
      next: res => {
        if (res.guid) {
          this.router.navigate(['dashboard']);
        } else {
          this.toastrService.error('Incorrect username or password', 'Login failed');
        }
      }
    });
  }
}
