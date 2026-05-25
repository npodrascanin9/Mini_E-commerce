import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { SelectOption } from "src/app/shared/options/select.options.model";
import { ProductService } from "../../services/product.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ShowErrorService } from "src/app/shared/errorDialog/services/showerror.service";
import { ActivatedRoute, Router } from "@angular/router";
import { CreateProductRequest } from "../../models/create.request.model";
import { UpdateProductRequest } from "../../models/update.request.model";
import { ProductCategoryOptionsService } from "src/app/shared/options/services/product-category.options.service";

@Component({
    selector: 'app-product',
    templateUrl: './product.component.html'
})

export class ProductComponent implements OnInit {

    id = Number(this.route.snapshot.paramMap.get('id'));
    isNew = !this.id;
    isEdit = !this.isNew;

    title = this.isNew
        ? 'Insert product'
        : 'Update product';

    form = new FormGroup({
        name: new FormControl<string>(
            '',
            [Validators.required]
        ),
        description: new FormControl<string>(''),
        price: new FormControl<number>(
            0,
            [
                Validators.required,
                Validators.min(1)
            ]
        ),
        productCategory: new FormControl<SelectOption | null>(
            null,
            [Validators.required]
        )
    });

    categoryOptions: SelectOption[] = [];

    isLoading = true;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private productService: ProductService,
        private showErrorService: ShowErrorService,
        private productCategoryOptionsService: ProductCategoryOptionsService,
        private matSnackBar: MatSnackBar
    ) { }

    ngOnInit(): void {
        this.init();
    }

    private init(): void {
        this.initCategoryOptions();
    }

    private initCategoryOptions(): void {
        this.productCategoryOptionsService.getOptions().subscribe(
            response => this.categoryOptions = response,
            error => this.displayError(error),
            () => this.initCreateEditData());
    }

    private initCreateEditData(): void {
        this.isLoading = this.isEdit;
        this.isEdit && this.getById();
    }

    private getById(): void {
        this.productService.getById(this.id).subscribe(
            response => this.initForm(response),
            error => this.displayError(error),
            () => this.isLoading = false);
    }

    private initForm(response: any): void {
        this.form.patchValue(response);
        response.productCategoryId && this.form.controls.productCategory.setValue(
            this.categoryOptions.find(x => x.key === response.productCategoryId) as SelectOption);
    }
    
    submit(): void {
        this.form.invalid
            ? this.matSnackBar.open('Invalid form')
            : this.save();
    }
    
    private save(): void {
        this.isLoading = true;
        this.isNew
            ? this.create()
            : this.update();
    }
    
    private create(): void {
        this.productService.create(this._createRequest()).subscribe(
            response => this.afterCreate(response.id),
            error => this.displayError(error));
    }
    
    private _createRequest(): CreateProductRequest {
        return new CreateProductRequest(
            this.form.value);
    }

    private afterCreate(id: number): void {
        this.matSnackBar.open(`Inserted record with Id='${id}'`);
        this.router.navigateByUrl(`/products/update/${id}`);
    }
    
    private update(): void {
        this.productService.updateById(this._updateRequest()).subscribe(
            () => this.matSnackBar.open('Successfully updated record'),
            error => this.displayError(error),
            () => this.onList());
    }

    private _updateRequest(): UpdateProductRequest {
        return new UpdateProductRequest(
            this.id, 
            this.form.value);
    }

    private displayError(error: any): void {
        this.isLoading = false;
        this.showErrorService.showError(error);
    }

    onList(): void {
        this.router.navigateByUrl('/products');
    }
}
