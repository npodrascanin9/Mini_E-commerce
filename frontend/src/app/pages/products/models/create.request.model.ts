export class CreateProductRequest {
    name: string;
    description: string;
    price: number;
    productCategoryId: number;

    constructor(formValue: any) {
        this.name = formValue.name;
        this.description = formValue.description || null;
        this.price = formValue.price;
        this.productCategoryId = formValue.productCategory?.key || null;
    }
}
