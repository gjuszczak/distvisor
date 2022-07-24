import { NgModule } from '@angular/core';
import { FileSizePipe } from './file-size.pipe';
import { AutofocusDirective } from './autofocus.directive';
import { UtilsService } from './utils.service';

@NgModule({
  imports: [],
  declarations: [FileSizePipe, AutofocusDirective],
  exports: [FileSizePipe, AutofocusDirective],
  providers: [UtilsService]
})
export class SharedModule { }
