import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { HomeBoxComponent } from 'src/home-box-depr/home-box/home-box.component';

const routes: Routes = [
  { path: 'home-box', component: HomeBoxComponent, pathMatch: 'full', canActivate: [MsalGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class HomeBoxRoutingModule {}