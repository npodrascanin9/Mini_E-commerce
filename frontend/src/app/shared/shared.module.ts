import { NgModule } from '@angular/core';
import { MaterialModule } from './material.module';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ErrorDialogModule } from './errorDialog/errorDialog.module';
import { FileExportingModule } from './fileExporting/fileExporing.module';
import { SelectOptionsModule } from './options/select-options.module';


@NgModule({
    exports: [
        MaterialModule,
        HttpClientModule,
        CommonModule,
        ReactiveFormsModule,
        ErrorDialogModule,
        FileExportingModule,
        SelectOptionsModule
    ]
})

export class SharedModule {}
