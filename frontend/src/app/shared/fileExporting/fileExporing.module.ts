import { NgModule } from "@angular/core";
import { FileExporterService } from "./services/fileExporter.service";

@NgModule({
    providers: [
        FileExporterService
    ]
})
export class FileExportingModule { }
