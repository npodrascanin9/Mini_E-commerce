import { Routes } from '@angular/router';
import { ProductsComponent } from './products.component';
import { ProductComponent } from './components/product/product.component';

export const PRODUCT_ROUTES: Routes = [
    {
        path: '',
        component: ProductsComponent
    },
    {
        path: 'create',
        component: ProductComponent
    },
    {
        path: 'update/:id',
        component: ProductComponent
    }
];
