import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { UpdateArticleForProductRequest } from "../models/update.request.model";
import { CreateArticleForProductRequest } from "../models/create.request.model";

@Injectable({
    providedIn: 'root'
})

export class ArticleForProductService {
    
    constructor(
        private http: HttpClient
    ) { }

    get(productId: number) {
        return this.http.get<any>(this._apiUrl(productId));
    }

    getById(productId: number, id: number) {
        return this.http.get<any>(`${this._apiUrl(productId)}/${id}`);
    }

    create(productId: number, request: CreateArticleForProductRequest) {
        return this.http.post<any>(
            this._apiUrl(productId), 
            request);
    }

    update(productId: number, request: UpdateArticleForProductRequest) {
        return this.http.put<any>(
            `${this._apiUrl(productId)}/${request.id}`,
            request);
    }

    deleteById(produdctId: number, id: number) {
        return this.http.delete<any>(`${this._apiUrl(produdctId)}/${id}`);
    }

    private _apiUrl(productId: number): string {
        return `${environment.apiUrl}/products/${productId}/articles`;
    }
}
