import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import { AdminRegionsComponent } from './admin-regions.component';

export const routes: Routes = [
  {
    path: '',
    component: AdminRegionsComponent,
    children: [
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
export class AdminRegionsRoutingModule {}
