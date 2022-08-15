import { NgModule } from '@angular/core';

import { SignalrService } from './signalr.service';
import { RfCodeService } from './rfcode.service';

@NgModule({
  providers: [
    SignalrService,
    RfCodeService,
  ]
})
export class SignalrModule { }
