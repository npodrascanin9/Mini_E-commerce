export class UpdateProductCategoryByIdRequest {
    id: number;
    name: string;
    description: string;

    constructor(id: number, formValue: any) {
        this.id = id;
        this.name = formValue.name;
        this.description = formValue.description;
    }
}
