import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/Services/product.service';
import { Product } from '../../Models/product.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
    products: Product[] = [];
    /**
     *
     */
    constructor(private productService: ProductService) {}

    ngOnInit(): void {
      this.loadProducts();
    }

    loadProducts(): void {
      this.productService.GetProducts().subscribe({
        next: (data) => this.products = data,
        error: (err) => console.error("Error loading products", err)
      });
    }

    addToCart(product: Product): void {
      console.log("adding product to cart");
    }

}
