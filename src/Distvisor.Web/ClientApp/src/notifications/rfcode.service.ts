import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class RfCodeService {
  rfCodeSubject$: Subject<string> = new Subject<string>();
}