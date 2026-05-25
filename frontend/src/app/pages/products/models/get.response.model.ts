import { GetProductsRowDto } from "./get.rowDto.model";

export interface GetProductsResponse {
    count: number;
    rows: GetProductsRowDto[];
}
