import { Component, OnInit } from '@angular/core';
import { ProductService } from './services/product.service';
import { ShowErrorService } from 'src/app/shared/errorDialog/services/showerror.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { GetProductsRowDto } from './models/get.rowDto.model';
import { GetProductsFilter } from './models/get.filter.model';
import { GetProductsResponse } from './models/get.response.model';
import { Router } from '@angular/router';
import { FileExporterService } from 'src/app/shared/fileExporting/services/fileExporter.service';

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html'
})

export class ProductsComponent implements OnInit {

    count = 0;
    dataSource = new MatTableDataSource<GetProductsRowDto>([]);
    displayedColumns = [
        'id',
        'name',
        'isActive',
        'productCategoryName',
        'price',
        'createdAt',
        'updatedAt',
        'actions'
    ];

    isLoading = true;

    constructor(
        private router: Router,
        private productService: ProductService,
        private showErrorService: ShowErrorService,
        private fileExporterService: FileExporterService,
        private matSnackBar: MatSnackBar
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
        this.productService.get(this._filter()).subscribe(
            response => this.mapResponse(response),
            error => this.displayError(error),
            () => this.isLoading = false);
    }

    private _filter(): GetProductsFilter {
        return new GetProductsFilter();
    }

    private mapResponse(response: GetProductsResponse): void {
        [this.dataSource.data, this.count] = [response.rows, response.count];
    }

    onDelete(row: GetProductsRowDto): void {
        this.canDelete(row) && this.productService.deleteById(row.id).subscribe(
            () => this.matSnackBar.open('Succesffully deleted record'),
            error => this.displayError(error),
            () => this.loadData());
    }

    private canDelete(row: GetProductsRowDto): boolean {
        return confirm(`Are you sure? Product with Id='${row.id}' will be deleted.`);
    }

    onExportExcel(): void {
        this.isLoading = true;
        this.exportExcelFile();
    }

    private exportExcelFile(): void {
        this.productService.exportExcel().subscribe(
            response => response && this.downloadExcel(response),
            error => this.displayError(error),
            () => this.isLoading = false);
    }

    private downloadExcel(file: any): void {
        this.fileExporterService.exportFile(file);
        this.matSnackBar.open('Succesfully exported file');
    }

    private displayError(error: any): void {
        this.isLoading = false;
        this.showErrorService.showError(error);
    }

    onCreate(): void {
        this.router.navigateByUrl('/products/create');
    }

    onUpdate(row: GetProductsRowDto): void {
        this.router.navigateByUrl(`/products/update/${row.id}`);
    }
}
