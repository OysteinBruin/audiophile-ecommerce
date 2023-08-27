import styles from '../styles/components/product-card.module.scss'
import Image from 'next/image'
import Link from "next/link";
import { PaginatedResult } from '../types/PaginatedResult'
import { Product } from '../types/Product'
async function getProducts() {
    const response = await fetch('http://localhost:5104/Catalog/items?itemCategory=speaker', {
        next: {
            revalidate: 0 // use 0 to opt out of using cache
        }
    })

    return await response.json()
}


export default async function ProductList() {
    const products = await getProducts();

    return (
        <>
            
            {products.data.map((product) => (
                <div key={product.id} className={styles.products__card}>
                    <div className={styles.products__imageContainer}>
                        {product.image != null &&
                            <Image
                                src={product.image.uri}
                                width={product.image.width}
                                height={product.image.height}
                                alt={"Product image"}>
                            </Image>
                        }
                        {product.image == null &&
                            <Image 
                                src="/next.svg" 
                                width="250"
                                height="250"
                                alt={"No image available"}>
                            </Image>
                        }
                    </div>
                    <div className={styles.products__textContainer}>
                        <div className={styles.products__textBlock}>
                            <h2 className={styles.products__h2}>{product.name}</h2>
                            <p className={styles.products__p}>{product.description}</p>
                            <Link href={`/speakers/${product.id}`}><a className="btn btn--peach">see product</a></Link>
                            <a
                                href="./product.html"
                                className="btn btn--peach"
                                data-link="product"
                                data-item="xx99-mark-one-headphones"
                                data-name="xx99 mark i"
                            >see product</a
                            >
                        </div>
                    </div>
                </div>
            ))}
            {products.failed || products.data?.length === 0 && (
                <p className="text-center">Sorry, no products available at the moment</p>
            )}
        </>
    )
}