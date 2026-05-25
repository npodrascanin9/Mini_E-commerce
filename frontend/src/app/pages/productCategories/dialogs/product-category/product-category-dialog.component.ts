import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProductCategoryService } from '../../services/productCategory.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CreateProductCategoryRequest } from '../../models/create.request.model';
import { UpdateProductCategoryByIdRequest } from '../../models/update.request.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GetProductCategoryByIdResponse } from '../../models/getById.response.model';
import { ShowErrorService } from 'src/app/shared/errorDialog/services/showerror.service';

@Component({
    selector: 'app-productCategory-dialog',
    templateUrl: './product-category-dialog.component.html'
})

export class ProductCategoryDialogComponent implements OnInit {

    isEdit = Boolean(this.data?.id);
    isNew = !this.isEdit;

    title = this.isNew 
        ? 'Insert category' 
        : 'Update category';

    form = new FormGroup({
        name: new FormControl<string>(
            '',
            [Validators.required]
        ),
        description: new FormControl('')
    });

    isLoading = true;

    constructor(
        private dialogRef: MatDialogRef<ProductCategoryDialogComponent>,
        @Inject(MAT_DIALOG_DATA) private data: {
            id: number
        },
        private productCategoryService: ProductCategoryService,
        private matSnackBar: MatSnackBar,
        private showErrorService: ShowErrorService
    ) { }

    ngOnInit(): void {
        this.init();
    }

    private init(): void {
        this.isNew
            ? this.isLoading = false
            : this.getById();
    }

    private getById(): void {
        this.productCategoryService.getById(this.data.id).subscribe(
            response => this.mapForm(response),
            error => this.displayError(error),
            () => this.isLoading = false);
    }

    private mapForm(response: GetProductCategoryByIdResponse): void {
        this.form.patchValue(response);
    }

    submit(): void {
        this.form.invalid
            ? this.matSnackBar.open('Form is invalid')
            : this.save();
    }

    private save(): void {
        this.isLoading = true;
        this.isNew
            ? this.create()
            : this.update()
    }

    private create(): void {
        this.productCategoryService.create(this._createRequest()).subscribe(
            response => this.matSnackBar.open(`Successfully inserted record with Id='${response?.id}'`),
            error => this.displayError(error),
            () => this.closeAfterSubmit());
    }

    private _createRequest(): CreateProductCategoryRequest {
        return new CreateProductCategoryRequest(
            this.form.value);
    }

    private update(): void {
        this.productCategoryService.updateById(this._updateRequest()).subscribe(
            () => this.matSnackBar.open('Successfully updated record'),
            error => this.displayError(error),
            () => this.closeAfterSubmit());
    }

    private _updateRequest(): UpdateProductCategoryByIdRequest {
        return new UpdateProductCategoryByIdRequest(
            this.data.id, 
            this.form.value);
    }

    private displayError(error: any): void {
        this.showErrorService.showError(error);
        this.isLoading = false;
    }

    closeDialog(): void {
        this.dialogRef.close();
    }

    private closeAfterSubmit(): void {
        this.isLoading = false;
        this.dialogRef.close({ isSubmitted: true });
    }
}
