import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RedirectionsComponent } from './redirections/redirections.component';

const routes: Routes = [
  { path: 'admin/redirections', component: RedirectionsComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RedirectionsRoutingModule {}
