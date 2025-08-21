import { Routes } from '@angular/router';
import { Login } from './login/login';
import { HomePage } from './home-page/home-page';
import { Createuser } from './createuser/createuser';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [

    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path:'login', component: Login  },
    { path:'home', component: HomePage ,canActivate : [AuthGuard]},
    {path:'create',component:Createuser,canActivate : [AuthGuard]}




];
