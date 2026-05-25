export interface ProductCategoryRowDto {
    id: number;
    name: string;
    description: string;
    isActive: boolean;
    createdAt: Date;
    updatedAt: Date;
    productsCount: number;
}
