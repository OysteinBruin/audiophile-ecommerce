import { Product } from "./Product";
import { Image } from "./Image";
export interface ProductDetail extends Product {
    body: string;
    price: number;
    qty: number;
    gallery: Image[];
}