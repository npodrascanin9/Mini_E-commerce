import { ArticleForProductDialogData } from "./articleDialog.data.model";

export class CreateArticleForProductRequest {
    productId: number;
    barcode: string | null;
    size: string | null;
    color: string | null;
    expirationDate: Date | null;

    constructor(
        data: ArticleForProductDialogData,
        formValue: any
    ) {
        this.productId = data.productId;
        this.barcode = formValue.barcode || null;
        this.size = formValue.size || null;
        this.color = formValue.color || null;
        this.expirationDate = formValue.expirationDate;
    }
}