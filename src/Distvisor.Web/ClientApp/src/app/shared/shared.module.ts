import { NgModule } from '@angular/core';
import { FileSizePipe } from './file-size.pipe';
import { AutofocusDirective } from './autofocus.directive';

@NgModule({
  imports: [],
  declarations: [FileSizePipe, AutofocusDirective],
  exports: [FileSizePipe, AutofocusDirective]
})
export class SharedModule { }
