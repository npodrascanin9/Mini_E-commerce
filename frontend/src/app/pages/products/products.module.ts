import { NgModule } from "@angular/core";
import { ProductsComponent } from "./products.component";
import { SharedModule } from "src/app/shared/shared.module";
import { RouterModule } from "@angular/router";
import { PRODUCT_ROUTES } from "./products.routes";
import { ProductService } from "./services/product.service";
import { ProductComponent } from "./components/product/product.component";
import { ArticleForProductService } from "./components/product/articles/services/articleForProduct.service";
import { ArticlesForProductComponent } from "./components/product/articles/articlesForProduct.component";
import { ArticleForProductDialogComponent } from "./components/product/articles/dialogs/detail/articleDialog.component";

@NgModule({
    declarations: [
        ProductsComponent,
        ProductComponent,
        ArticlesForProductComponent,
        ArticleForProductDialogComponent
    ],
    imports: [
        SharedModule,
        RouterModule.forChild(PRODUCT_ROUTES)
    ],
    providers: [
        ProductService,
        ArticleForProductService
    ]
})

export class ProductsModule { }
