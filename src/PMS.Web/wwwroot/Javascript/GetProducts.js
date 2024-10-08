// getProducts.js

let products = [];

export async function getAllProducts() {
    const res = await fetch("https://localhost:7225/api/Product/products");
    const data = await res.json();
    products = data;
    
    return products;  // Return the products instead of calling renderAllProducts
}

