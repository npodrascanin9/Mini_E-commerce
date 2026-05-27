import { Component, Input, OnInit } from "@angular/core";
import { ArticleForProductService } from "./services/articleForProduct.service";
import { ShowErrorService } from "src/app/shared/errorDialog/services/showerror.service";
import { MatTableDataSource } from "@angular/material/table";
import { MatDialog } from "@angular/material/dialog";
import { ArticleForProductDialogComponent } from "./dialogs/detail/articleDialog.component";
import { ArticleForProductDialogData } from "./models/articleDialog.data.model";
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
    selector: 'app-articlesForProduct',
    templateUrl: './articlesForProduct.component.html'
})

export class ArticlesForProductComponent implements OnInit {

    @Input() productId!: number; 

    count = 0;
    dataSource = new MatTableDataSource<any>([]);
    displayedColumns = [
        'id',
        'productName',
        'barcode',
        'size',
        'expirationDate',
        'isActive',
        'createdAt',
        'updatedAt',
        'actions'
    ];

    isLoading = true;

    constructor(
        private dialog: MatDialog,
        private matSnackBar: MatSnackBar,
        private articleForProductService: ArticleForProductService,
        private showErrorService: ShowErrorService
    ) { }

    ngOnInit(): void {
        this.init();
    }

    private init(): void {
        this.loadData();
    }

    loadData(): void {
        this.isLoading = true;
        this.get();
    }

    private get(): void {
        this.articleForProductService.get(this.productId).subscribe(
            response => this.mapResponse(response),
            error => this.showError(error),
            () => this.isLoading = false);
    }

    private mapResponse(response: any): void {
        this.dataSource.data = response.rows;
        this.count = response.count;
    }

    onCreate(): void {
        this.dialog.open(ArticleForProductDialogComponent, {
            data: this._createDialogData()
        }).afterClosed().subscribe(result => result?.isSubmitted && this.loadData());
    }

    private _createDialogData() {
        return new ArticleForProductDialogData(this.productId, 0);
    }

    onUpdate(row: any): void {
        this.dialog.open(ArticleForProductDialogComponent, {
            data: this._updateDialogData(row)
        }).afterClosed().subscribe(result => result?.isSubmitted && this.loadData());
    }

    private _updateDialogData(row: any) {
        return new ArticleForProductDialogData(
            this.productId,
            row.id);
    }

    onDelete(row: any): void {
        this.canDelete(row) && this.deleteById(row.id);
    }

    private canDelete(row: any): boolean {
        return row?.id && confirm(`Are you sure? Article with id='${row.id}' will be deleted`);
    }

    private deleteById(id: number): void {
        this.isLoading = true;
        this.articleForProductService.deleteById(this.productId, id).subscribe(
            () => this.matSnackBar.open(`Succesffully deleted article with Id='${id}'`),
            error => this.showError(error),
            () => this.loadData());
    }

    private showError(error: any): void {
        this.isLoading = false;
        this.showErrorService.showError(error);
    }
}
