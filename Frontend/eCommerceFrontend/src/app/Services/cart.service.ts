import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { tap } from 'rxjs/operators';

export interface CartItem
{
    id: string,
    cartId: string,
    productId: string,
    quantity: number,
}

export interface Cart{
    id: string,
    totalPrice: number,
    cartItems: CartItem[]
}

export interface AddRemoveCartItems
{
    productId: string,
    quantity: number
}

@Injectable({
    providedIn: "root"
})
export class CartService {
    private cartState = new BehaviorSubject<Cart | null>(null);
    cart$ = this.cartState.asObservable();

    /**
     *
     */
    constructor(private http: HttpClient) {}

    addProductToCart(userId: string , products: AddRemoveCartItems[]) : Observable<Cart> {
        return this.http.post<Cart>(`http://localhost:5152/api/cart/${userId}`, products)
        .pipe(
            tap(cart => this.cartState.next(cart))
        );
    }

    getUserCart(userId: string) : Observable<Cart>{
        return this.http.get<Cart>(`http://localhost:5152/api/cart/${userId}`)
        .pipe(
            tap(cart => this.cartState.next(cart))
        )
    }

    clearCart(): void
    {
        this.cartState.next(null);
    }

    get cart(): Cart | null {
        return this.cartState.value;
    }
}