import { NgModule } from "@angular/core";
import { ProductCategoriesComponent } from "./productCategories.component";
import { RouterModule } from "@angular/router";
import { SharedModule } from 'src/app/shared/shared.module';
import { PRODUCT_CATEGORY_ROUTES } from "./productCategories.routing";
import { ProductCategoryService } from "./services/productCategory.service";
import { ProductCategoryDialogComponent } from "./dialogs/product-category/product-category-dialog.component";


@NgModule({
    imports: [
        SharedModule,
        RouterModule.forChild(PRODUCT_CATEGORY_ROUTES)
    ],
    declarations: [
        ProductCategoriesComponent,
        ProductCategoryDialogComponent
    ],
    providers: [
        ProductCategoryService
    ]
})

export class ProductCategoriesModule { }
