import { Component, inject, signal } from '@angular/core';
import { TaskService } from '../task.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent {
  private taskService = inject(TaskService);

  isAddingTask = signal(false);
  tasks = this.taskService.tasks

  onStartAddTask(): void {
    this.isAddingTask.set(true);
  }

  onFinishAddTask(): void {
    this.isAddingTask.set(false);
  }
}
