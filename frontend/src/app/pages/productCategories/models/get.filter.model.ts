export class GetProductCategoriesFilter {
    name: string;
    isActive: boolean;

    constructor(formValue: any) {
        this.name = formValue.name;
        this.isActive = formValue.isActive;
    }
}
