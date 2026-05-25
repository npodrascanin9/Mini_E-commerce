export class CreateProductCategoryRequest {
    name: string;
    description: string;

    constructor(formValue: any) {
        this.name = formValue.name;
        this.description = formValue.description;
    }
}
