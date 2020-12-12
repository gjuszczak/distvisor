import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-import-financial-files-dialog',
  templateUrl: './import-financial-files-dialog.component.html'
})
export class ImportFinancialFilesDialogComponent implements OnChanges, OnDestroy {
  @Input() isVisible: boolean = false;
  @Output() onHide: EventEmitter<any> = new EventEmitter();

  private subscriptions: Subscription[] = [];

  uploadedFiles: any[] = [];

  constructor(private messageService: MessageService) {
    this.uploadedFiles = this.initUploadedFiles();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isVisible']) {
      this.uploadedFiles = this.initUploadedFiles();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  onUpload(event: any) {
    for (let file of event.files) {
      this.uploadedFiles.push(file);
    }
  }

  onCancelClicked() {
    this.onHide.emit();
  }

  initUploadedFiles = () => <any>[];
}