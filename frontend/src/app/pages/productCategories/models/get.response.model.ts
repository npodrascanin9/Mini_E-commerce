import { ProductCategoryRowDto } from "./rowDto.response.model";

export interface GetProductCategoriesResponse {
    count: number;
    rows: ProductCategoryRowDto[];
}
