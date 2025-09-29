import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs";
import { Product } from "../Models/product.model";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: "root"
})
export class ProductService {
    private apiUrl = environment.apiUrl;

    /**
     *
     */
    constructor(private http: HttpClient) { }

    GetProducts(): Observable<Product[]> {
        return this.http.get<Product[]>(this.apiUrl);
    }
}