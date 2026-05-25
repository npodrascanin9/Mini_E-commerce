import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { SelectOption } from "../select.options.model";

@Injectable({
    providedIn: 'root'
})

export class ProductCategoryOptionsService {
    
    private apiUrl = `${environment.apiUrl}/options/productCategory`;

    constructor(
        private http: HttpClient
    ) { }

    getOptions() {
        return this.http.get<SelectOption[]>(this.apiUrl);
    }
}
