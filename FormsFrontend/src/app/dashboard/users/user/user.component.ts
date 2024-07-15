import { Component, inject, input, OnInit, signal } from '@angular/core';
import { TaskService } from '../../tasks/task.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { User, UserService } from '../user.service';
import { Observable } from 'rxjs';
import { RegistrationResponse } from '../../../account.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {
  user = input.required<User>();
  isEditable = signal(false);

  form!: FormGroup<{
    firstName: FormControl,
    lastName: FormControl,
    email: FormControl
  }>;

  private userService = inject(UserService);
  private toastrService = inject(ToastrService);
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

  constructor() {
    this.initForm();
  }

  initForm() {
    this.form = new FormGroup({
      firstName: new FormControl({ value: '', disabled: true },
        [Validators.required, Validators.maxLength(100)]
      ),
      lastName: new FormControl({ value: '', disabled: true },
        [Validators.required, Validators.maxLength(100)]
      ),
      email: new FormControl({ value: '', disabled: true },
        [Validators.required, Validators.email, Validators.maxLength(100)]
      ),
    });
  }

  ngOnInit(): void {
    this.updateForm();
  }

  onEdit(): void {
    this.form.controls.firstName.enable();
    this.form.controls.lastName.enable();
    this.form.controls.email.enable();
    this.isEditable.set(true);
  }

  onCancel(): void {
    this.updateForm();
    this.disableEdit();
  }

  onSave(): void {
    this.isSubmitted = true;

    if (this.form.invalid) {
      return;
    }

    this.forwardInput().subscribe({
      next: res => this.handleResponse(res)
    });
  }

  onDelete(): void {
    this.userService.deleteUser(this.user().guid).subscribe();
  }

  private forwardInput(): Observable<RegistrationResponse> {
    const enteredFirstName = this.form.value.firstName!;
    const enteredLastName = this.form.value.lastName!;
    const enteredEmail = this.form.value.email!;

    return this.userService.updateUser({
      guid: this.user().guid,
      firstName: enteredFirstName,
      lastName: enteredLastName,
      email: enteredEmail,
    });
  }

  private handleResponse(response: RegistrationResponse): void {
    if (response.succeeded) {
      this.toastrService.success('User info was successfully saved', 'User info edit');
      this.disableEdit();
    } else {
      this.toastrService.error(response.errors[0].description, 'User info edit');
    }
  }

  private disableEdit(): void {
    this.form.controls.firstName.disable();
    this.form.controls.lastName.disable();
    this.form.controls.email.disable();
    this.isEditable.set(false);
  }

  private updateForm(): void {
    this.form.patchValue({
      firstName: this.user().firstName,
      lastName: this.user().lastName,
      email: this.user().email
    })
  }
}
