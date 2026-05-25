export class UpdateProductRequest {
    id: number;
    name: string;
    description: string;
    price: number;
    productCategoryId: number;

    constructor(id: number, formValue: any) {
        this.id = id;
        this.name = formValue.name;
        this.description = formValue.description || null;
        this.price = formValue.price;
        this.productCategoryId = formValue.productCategory?.key || null;
    }
}
