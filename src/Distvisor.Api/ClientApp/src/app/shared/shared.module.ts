import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ProgressBarModule } from 'primeng/progressbar';
import { ConfirmModalComponent } from './modal/confirm-modal/confirm-modal.component';
import { FileSizePipe } from './file-size.pipe';
import { InputModalComponent } from './modal/input-modal/input-modal.component';

@NgModule({
  imports: [
    CommonModule,
    ButtonModule,
    DialogModule,
    ProgressBarModule,
  ],
  declarations: [
    ConfirmModalComponent,
    FileSizePipe,
    InputModalComponent,
  ],
  exports: [
    ConfirmModalComponent,
    FileSizePipe,
    InputModalComponent,
  ]
})
export class SharedModule { }
