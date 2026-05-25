import { NgModule } from "@angular/core";
import { ErrorDialogComponent } from "./components/errorDialog.component";
import { MaterialModule } from "../material.module";
import { ShowErrorService } from "./services/showerror.service";

@NgModule({
    declarations: [
        ErrorDialogComponent
    ],
    imports: [
        MaterialModule
    ],
    providers: [
        ShowErrorService
    ]
})

export class ErrorDialogModule { }
