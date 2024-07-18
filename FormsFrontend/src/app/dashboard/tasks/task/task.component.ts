import { Component, inject, input, OnInit, signal } from '@angular/core';
import { Task, TaskService } from '../task.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { notPastDate } from '../add-task/add-task.component';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrl: './task.component.css'
})
export class TaskComponent implements OnInit {
  task = input.required<Task>();
  isEditable = signal(false);

  form!: FormGroup<{
    title: FormControl,
    description: FormControl,
    dueDate: FormControl
  }>;

  private taskService = inject(TaskService);
  private toastrService = inject(ToastrService);
  private isSubmitted = false;

  get titleInvalid(): boolean {
    return (this.isSubmitted ||
      (this.form.controls.title.touched &&
      this.form.controls.title.dirty)) &&
      this.form.controls.title.invalid;
  }

  get descriptionInvalid(): boolean {
    return (this.isSubmitted ||
      (this.form.controls.description.touched &&
      this.form.controls.description.dirty)) &&
      this.form.controls.description.invalid;
  }

  get dueDateInvalid(): boolean {
    return (this.isSubmitted ||
      (this.form.controls.dueDate.touched &&
      this.form.controls.dueDate.dirty)) &&
      this.form.controls.dueDate.invalid;
  }

  constructor() {
    this.initForm();
  }

  initForm(): void {
    this.form = new FormGroup({
      title: new FormControl({ value: '', disabled: true },
        [Validators.required, Validators.maxLength(100)]
      ),
      description: new FormControl({ value: '', disabled: true },
        [Validators.maxLength(1000)]
      ),
      dueDate: new FormControl({ value: '', disabled: true },
        [Validators.required, notPastDate]
      ),
    })
  }

  ngOnInit(): void {
    this.updateForm();
  }

  onEdit(): void {
    this.form.controls.title.enable();
    this.form.controls.description.enable();
    this.form.controls.dueDate.enable();
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

    const response = this.forwardInput();

    response.subscribe({
      next: res => {
          this.toastrService.success('Task was successfully saved', 'Task edit');
          this.disableEdit();
        }
    });
  }

  onDelete(): void {
    this.taskService.deleteTask(this.task().id).subscribe();
  }

  private forwardInput(): Observable<Object> {
    const enteredTitle = this.form.value.title!;
    const enteredDescription = this.form.value.description ?? "";
    const enteredDueDate = this.form.value.dueDate!;

    return this.taskService.updateTask({
      id: this.task().id,
      userId: this.task().userId,
      title: enteredTitle,
      description: enteredDescription,
      creationDate: this.task().creationDate,
      modificationDate: this.task().modificationDate,
      dueDate: enteredDueDate,
    });
  }

  private disableEdit() {
    this.form.controls.title.disable();
    this.form.controls.description.disable();
    this.form.controls.dueDate.disable();
    this.isEditable.set(false);
  }

  private updateForm() {
    this.form.patchValue({
      title: this.task().title,
      description: this.task().description,
      dueDate: this.task().dueDate
    })
  }
}
