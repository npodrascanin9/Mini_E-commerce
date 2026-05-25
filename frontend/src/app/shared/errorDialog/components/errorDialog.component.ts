import { Component, Inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
  selector: 'app-error-dialog',
  template: `
    <h2 mat-dialog-title>Server Error</h2>
    <h3 mat-dialog-subtitle>{{ code }}</h3>
    <mat-dialog-content>
      <p>{{ description }}</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Close</button>
    </mat-dialog-actions>
  `
})
export class ErrorDialogComponent implements OnInit {

    code?: string;
    description?: string;

    constructor(
        @Inject(MAT_DIALOG_DATA) public error: any
    ) {}

    ngOnInit(): void {
        console.log(this.error);
        this.init();
    }

    private init() {
        this.description = this.error?.error?.description || 'Server error';
        this.code = this.error?.error?.code || 'Unknown code error';
    }
}
