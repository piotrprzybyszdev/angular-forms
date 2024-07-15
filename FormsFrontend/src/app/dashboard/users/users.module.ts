import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";

import { UsersRoutingModule } from "./users-routing.module";
import { UserListComponent } from "./user-list/user-list.component";
import { UserComponent } from "./user/user.component";
import { AddUserComponent } from "./add-user/add-user.component";
import { SharedModule } from "../../shared/shared.module";

@NgModule({
    declarations: [UserComponent, AddUserComponent, UserListComponent],
    imports: [SharedModule, UsersRoutingModule, ReactiveFormsModule]
})
export class UsersModule {

}
