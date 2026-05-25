import { Component, Inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ArticleForProductService } from "../../services/articleForProduct.service";
import { FormControl, FormGroup } from "@angular/forms";
import { ArticleForProductDialogData } from "../../models/articleDialog.data.model";
import { ShowErrorService } from "src/app/shared/errorDialog/services/showerror.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { CreateArticleForProductRequest } from "../../models/create.request.model";
import { UpdateArticleForProductRequest } from "../../models/update.request.model";

@Component({
    selector: 'app-articleForProductDialog',
    templateUrl: './articleDialog.component.html'
})

export class ArticleForProductDialogComponent implements OnInit {

    isNew = !(this.data.id > 0);
    title = this.isNew
        ? `Insert article for product with Id='${this.data.productId}'`
        : `Update article for product with Id='${this.data.productId}'`;

    form = new FormGroup({
        barcode: new FormControl<string>(''),
        color: new FormControl<string>(''),
        size: new FormControl<string>(''),
        expirationDate: new FormControl<Date | null>(null)
    });

    isLoading = true;

    constructor(
        @Inject(MAT_DIALOG_DATA) private data: ArticleForProductDialogData,
        private dialogRef: MatDialogRef<ArticleForProductDialogComponent>,
        private articleForProductService: ArticleForProductService,
        private showErrorService: ShowErrorService,
        private matSnackBar: MatSnackBar
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
        this.articleForProductService.getById(this.data.productId, this.data.id).subscribe(
            response => this.mapForm(response),
            error => this.showError(error),
            () => this.isLoading = false);
    }

    private mapForm(response: any): void {
        this.form.patchValue(response);
    }

    submit(): void {
        this.form.invalid
            ? this.matSnackBar.open('Invalid form')
            : this.save();
    }

    private save(): void {
        this.isLoading = true;
        this.isNew ? this.create() : this.update();
    }

    private create(): void {
        this.articleForProductService.create(this.data.productId, this._createRequest()).subscribe(
            response => this.matSnackBar.open(`Inserted article with Id='${response.id}'`),
            error => this.showError(error),
            () => this.closeAfterSubmit());
    }

    private _createRequest() {
        return new CreateArticleForProductRequest(
            this.data,
            this.form.value);
    }

    private update(): void {
        this.articleForProductService.update(this.data.productId, this._updateRequest()).subscribe(
            () => this.matSnackBar.open(`Inserted article with Id='${this.data.id}'`),
            error => this.showError(error),
            () => this.closeAfterSubmit());
    }

    private _updateRequest() {
        return new UpdateArticleForProductRequest(
            this.data,
            this.form.value);
    }

    private closeAfterSubmit(): void {
        this.dialogRef.close({ isSubmitted: true });
    }

    private showError(error: any): void {
        this.isLoading = false;
        this.showErrorService.showError(error);
    }

    closeDialog(): void {
        this.dialogRef.close();
    }
}
