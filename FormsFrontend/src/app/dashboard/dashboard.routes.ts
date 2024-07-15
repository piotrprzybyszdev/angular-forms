import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: HomeComponent,
        title: 'Home'
    },
    {
        path: 'users',
        loadChildren: () => import('./users/users.module').then(mod => mod.UsersModule),
        title: 'Users'
    },
    {
        path: 'tasks',
        loadChildren: () => import('./tasks/tasks.module').then(mod => mod.TasksModule),
        title: 'Tasks'
    }
];
