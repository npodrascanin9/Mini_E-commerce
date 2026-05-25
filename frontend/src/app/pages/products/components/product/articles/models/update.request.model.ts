import { ArticleForProductDialogData } from "./articleDialog.data.model";

export class UpdateArticleForProductRequest {
    id: number;
    productId: number;
    barcode: string | null;
    size: string | null;
    color: string | null;
    expirationDate: Date | null;

    constructor(
        data: ArticleForProductDialogData,
        formValue: any
    ) {
        this.id = data.id;
        this.productId = data.productId;
        this.barcode = formValue.barcode || null;
        this.size = formValue.size || null;
        this.color = formValue.color || null;
        this.expirationDate = formValue.expirationDate;
    }
}
