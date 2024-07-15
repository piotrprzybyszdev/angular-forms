import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { TasksRoutingModule } from "./tasks-routing.module";

import { TaskListComponent } from "./task-list/task-list.component";
import { TaskComponent } from "./task/task.component";
import { AddTaskComponent } from "./add-task/add-task.component";
import { SharedModule } from "../../shared/shared.module";

@NgModule({
    declarations: [TaskListComponent, TaskComponent, AddTaskComponent],
    imports: [SharedModule, TasksRoutingModule, ReactiveFormsModule]
})
export class TasksModule {

}
