import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { DataViewModule } from 'primeng/dataview';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { InplaceModule } from 'primeng/inplace';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { RippleModule } from 'primeng/ripple';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';

import { ApiModule } from 'src/app/api/api.module';
import { SharedModule } from 'src/app/shared';
import { HomeBoxRoutingModule } from './home-box-routing.module';

import { HomeBoxComponent } from './home-box/home-box.component';
import { DevicesListComponent } from './devices-list/devices-list.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,

    // PrimeNg
    ButtonModule,
    CardModule,
    CodeHighlighterModule,
    ConfirmPopupModule,
    DataViewModule,
    DialogModule,
    DropdownModule,
    InplaceModule,
    InputNumberModule,
    InputSwitchModule,
    InputTextareaModule,
    InputTextModule,
    RippleModule,
    TableModule,
    TabViewModule,

    // internal
    ApiModule,
    SharedModule,
    HomeBoxRoutingModule,
  ],
  declarations: [
    HomeBoxComponent,
    DevicesListComponent
  ],
  providers: [ConfirmationService]
})
export class HomeBoxModule { }
