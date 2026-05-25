import { Component, OnInit } from "@angular/core";
import { ProductCategoryService } from "./services/productCategory.service";
import { ProductCategoryRowDto } from "./models/rowDto.response.model";
import { MatTableDataSource } from "@angular/material/table";
import { GetProductCategoriesResponse } from "./models/get.response.model";
import { GetProductCategoriesFilter } from "./models/get.filter.model";
import { FormControl, FormGroup } from "@angular/forms";
import { CHECKBOX_FILTER_OPTIONS } from "src/app/shared/options/checkbox-filter.options";
import { MatDialog } from "@angular/material/dialog";
import { ProductCategoryDialogComponent } from "./dialogs/product-category/product-category-dialog.component";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ShowErrorService } from "src/app/shared/errorDialog/services/showerror.service";

@Component({
    selector: 'app-productCategories',
    templateUrl: './productCategories.component.html'
})

export class ProductCategoriesComponent implements OnInit {

    checkboxFilterOptions = CHECKBOX_FILTER_OPTIONS;

    form = new FormGroup({
        name: new FormControl<string>(''),
        isActive: new FormControl<boolean | null>(true)
    });

    dataSource = new MatTableDataSource<ProductCategoryRowDto>([]);
    count = 0;

    displayedColumns = [
        'id',
        'name',
        'isActive',
        'productsCount',
        'createdAt',
        'updatedAt',
        'actions'
    ];

    filteredColumns = this.displayedColumns.map(
        column => `filter-${column}`);


    isLoading = true;

    constructor(
        private dialog: MatDialog,
        private matSnackBar: MatSnackBar,
        private showErrorService: ShowErrorService,
        private productCategoryService: ProductCategoryService
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
        this.productCategoryService.get(this._filter()).subscribe(
            response => this.mapResponse(response),
            error => this.displayError(error),
            () => this.isLoading = false);
    }

    private _filter(): GetProductCategoriesFilter {
        return new GetProductCategoriesFilter(
            this.form.value);
    }

    private mapResponse(response: GetProductCategoriesResponse): void {
        [this.dataSource.data, this.count] = [response.rows, response.count];
    }

    onCreate(): void {
        this.dialog
            .open(ProductCategoryDialogComponent)
            .afterClosed().subscribe(
                result => result?.isSubmitted && this.loadData());
    }

    onUpdate(row: ProductCategoryRowDto): void {
        this.dialog.open(ProductCategoryDialogComponent, {
            data: { id: row.id }
        }).afterClosed().subscribe(
            result => result?.isSubmitted && this.loadData());
    }

    onDelete(row: ProductCategoryRowDto): void {
        this.canDelete(row) && this.deleteById(row.id);
    }

    private canDelete(row: ProductCategoryRowDto): boolean {
        return confirm(`Are you sure? Product category with Id='${row.id}' will be deleted`);
    }

    private deleteById(id: number): void {
        this.isLoading = true;
        this.productCategoryService.deleteById(row.id).subscribe(
            () => this.matSnackBar.open('Successfully deleted record'),
            error => this.displayError(error),
            () => this.loadData());
    }

    private displayError(error: any): void {
        this.showErrorService.showError(error);
        this.isLoading = false;
    }
}
