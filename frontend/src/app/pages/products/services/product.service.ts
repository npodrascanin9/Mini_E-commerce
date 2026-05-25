import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { GetProductsResponse } from "../models/get.response.model";
import { GetProductsFilter } from "../models/get.filter.model";
import { CreateProductRequest } from "../models/create.request.model";
import { UpdateProductRequest } from "../models/update.request.model";

@Injectable({
    providedIn: 'root'
})

export class ProductService {

    private apiurl = `${environment.apiUrl}/products`;

    constructor(
        private http: HttpClient
    ) { }

    get(filter: GetProductsFilter) {
        return this.http.get<GetProductsResponse>(this.apiurl, {
            params: {
                searchOptions: JSON.stringify(filter || {})
            }
        });
    }

    getById(id: number) {
        return this.http.get(`${this.apiurl}/${id}`);
    }

    create(request: CreateProductRequest) {
        return this.http.post<any>(this.apiurl, request);
    }

    updateById(request: UpdateProductRequest) {
        return this.http.put<any>(`${this.apiurl}/${request.id}`, request);
    }

    exportExcel() {
        return this.http.get<any>(`${this.apiurl}/exportExcel`);
    }

    deleteById(id: number) {
        return this.http.delete(`${this.apiurl}/${id}`);
    }
}
