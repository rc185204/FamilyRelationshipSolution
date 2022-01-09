import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FirstComponent } from './first/first.component';
import { SecondComponent } from './second/second.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

import { ChildaComponent } from './childa/childa.component';
import { ChildbComponent } from './childb/childb.component';
import { AdminComponent } from './admin/admin/admin.component';
import { AuthGuard } from './auth/auth.guard';

import { NameEditorComponent } from './name-editor/name-editor.component';
import { ProfileEditorComponent } from './profile-editor/profile-editor.component';

const routes: Routes = [
  { path: 'app-authentication', component: AuthenticationComponent   },
  { path: 'first-component', component: FirstComponent ,
    canActivate: [AuthGuard], // 守护路由，判断授权状态
    children: [ // 二级路由
    {
      path: 'app-childa', // child route path
      component: ChildaComponent, // child route component that the router renders
    },
    {
      path: 'app-childb',
      component: ChildbComponent, // another child route component that the router renders
    },
  ], },
  { path: 'second-component', component: SecondComponent },
  { path: 'app-admin', component: AdminComponent, },
  { path: 'app-name-editor', component: NameEditorComponent },
  { path: 'app-profile-editor', component: ProfileEditorComponent },
  { path: '',   redirectTo: '/app-authentication', pathMatch: 'full' }, // redirect to `app-authentication` 重定向路由
  { path: '**', component: PageNotFoundComponent },  // Wildcard route for a 404 page 通配符路由
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
