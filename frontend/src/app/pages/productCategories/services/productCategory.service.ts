import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { GetProductCategoriesResponse } from "../models/get.response.model";
import { HttpClient } from "@angular/common/http";
import { GetProductCategoriesFilter } from "../models/get.filter.model";
import { CreateProductCategoryRequest } from "../models/create.request.model";
import { UpdateProductCategoryByIdRequest } from "../models/update.request.model";
import { GetProductCategoryByIdResponse } from "../models/getById.response.model";

@Injectable({
    providedIn: 'root'
})

export class ProductCategoryService {

    private apiUrl = `${environment.apiUrl}/productCategories`;

    constructor(
        private http: HttpClient
    ) { }

    get(filter: GetProductCategoriesFilter) : Observable<GetProductCategoriesResponse> {
        return this.http.get<GetProductCategoriesResponse>(
            this.apiUrl,
        {
            params: {
                searchOptions: JSON.stringify(filter || {})
            }
        });
    }

    getById(id: number) {
        return this.http.get<GetProductCategoryByIdResponse>(
            `${this.apiUrl}/${id}`);
    }

    create(request: CreateProductCategoryRequest) {
        return this.http.post<any>(this.apiUrl, request);
    }

    updateById(request: UpdateProductCategoryByIdRequest) {
        return this.http.put<any>(`${this.apiUrl}/${request.id}`, request);
    }

    deleteById(id: number) {
        return this.http.delete(`${this.apiUrl}/${id}`);
    }
}
