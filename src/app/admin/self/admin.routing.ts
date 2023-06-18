import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {AdminComponent} from './admin.component';
import { AdminAuthComponent } from '../auth/admin-auth.component';

export const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: 'auth',
        component: AdminAuthComponent
      },

      {
        path: 'regions',
        loadChildren: () => import('../regions/admin-regions.module').then((m) => m.AdminRegionsModule),
      },
      
      {
        path: '**',
        redirectTo: 'list',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
