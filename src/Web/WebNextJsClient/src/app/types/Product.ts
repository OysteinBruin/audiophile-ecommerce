import { Image } from "./Image";
export interface Product {
    id: number;
    name: string;
    description: string;
    image?: Image;
    updatedAt: Date;
}