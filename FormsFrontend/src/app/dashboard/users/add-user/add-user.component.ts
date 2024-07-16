import { Component, inject, output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../user.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrl: './add-user.component.css'
})
export class AddUserComponent {
  close = output();
  form!: FormGroup<{
    firstName: FormControl,
    lastName: FormControl,
    email: FormControl,
    password: FormControl
  }>;

  private userService = inject(UserService);
  private toastrService = inject(ToastrService)
  private isSubmitted = false;

  get firstNameInvalid(): boolean {
    return (this.isSubmitted ||
      (this.form.controls.firstName.touched &&
      this.form.controls.firstName.dirty)) &&
      this.form.controls.firstName.invalid;
  }

  get lastNameInvalid(): boolean {
    return (this.isSubmitted ||
      (this.form.controls.lastName.touched &&
      this.form.controls.lastName.dirty)) &&
      this.form.controls.lastName.invalid;
  }

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

  constructor() {
    this.initForm();
  }

  initForm(): void {
    this.form = new FormGroup({
      firstName: new FormControl('', {
        validators: [Validators.required, Validators.maxLength(100)]
      }),
      lastName: new FormControl('', {
        validators: [Validators.required, Validators.maxLength(100)]
      }),
      email: new FormControl('', {
        validators: [Validators.email, Validators.required, Validators.maxLength(100)]
      }),
      password: new FormControl('', {
        validators: [Validators.required, Validators.maxLength(100)]
      })
    });
  }

  onCancel(): void {
    this.close.emit();
  }

  onSubmit(): void {
    this.isSubmitted = true;

    if (this.form.invalid) {
      return;
    }

    const enteredFirstName = this.form.value.firstName!;
    const enteredLastName = this.form.value.lastName!;
    const enteredEmail = this.form.value.email!;
    const enteredPassword = this.form.value.password!;

    this.userService.addUser(
      enteredFirstName, enteredLastName, enteredEmail, enteredPassword
    ).subscribe({
      next: res => {
        this.close.emit();
        this.toastrService.success('User was successfully added', 'User added');
      },
      error: res => {
        this.toastrService.error(res.errors[0].details, 'User add');
      }
    });
  }
}
