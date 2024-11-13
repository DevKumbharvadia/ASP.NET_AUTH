import { Routes } from '@angular/router';
import { LoginComponent } from './component/login/login.component';
import { AdminComponent } from './component/admin/admin.component';
import { authGuard } from './guard/auth.guard';
import { RegisterComponent } from './component/register/register.component';
import { HomeComponent } from './component/home/home.component';
import { PageLayoutComponent } from './component/page-layout/page-layout.component';
import { UserViewComponent } from './component/user-view/user-view.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: '',
    component: LoginComponent,
  },
  {
    path: 'signup',
    component: RegisterComponent,
  },
  {
    path: 'layout',
    component: PageLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'admin',
        component: AdminComponent,
      },
      {
        path: 'UserView',
        component: UserViewComponent,
      },
      {
        path: 'home',
        component: HomeComponent,
      },
    ],
  },
];
