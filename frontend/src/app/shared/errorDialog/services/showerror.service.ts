import { Injectable } from "@angular/core";
import { ErrorDialogComponent } from "../components/errorDialog.component";
import { MatDialog } from "@angular/material/dialog";

@Injectable({ providedIn: 'root' })
export class ShowErrorService {
    constructor(
        private dialog: MatDialog
    ) {}

    showError(error: any): void {
        this.dialog.open(ErrorDialogComponent, {
            data: error,
            width: '400px'
        });
    }
}
