import { Directive, ElementRef, Input } from "@angular/core";

@Directive({
  selector: "[app-autofocus]"
})
export class AutofocusDirective {
  private focus = true;

  constructor(private el: ElementRef) {
  }

  ngOnInit() {
    if (this.focus) {
      //Otherwise Angular throws error: Expression has changed after it was checked.
      window.setTimeout(() => {
        //For SSR (server side rendering) this is not safe. Use: https://github.com/angular/angular/issues/15008#issuecomment-285141070)
        this.el.nativeElement.focus(); 
      });
    }
  }

  @Input() set autofocus(condition: boolean) {
    this.focus = condition !== false;
  }
}