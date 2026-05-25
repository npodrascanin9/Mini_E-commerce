export interface GetProductsRowDto {
    id: number;
    name: string;
    description: string;
    price: number;
    productCategoryId: number;
    productCategoryName: string;
    isActive: boolean;
    createdAt: Date;
    updatedAt: Date;
}
