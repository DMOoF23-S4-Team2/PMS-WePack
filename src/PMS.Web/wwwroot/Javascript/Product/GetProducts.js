import { showMessage } from "../../Components/MessageBox.js";

let products = [];

export async function getAllProducts() {
    try {
        const res = await fetch("https://localhost:7225/api/Product/products");

        if (!res.ok) {
            throw new Error(`HTTP error! status: ${res.status}`);
        }

        const data = await res.json();
        products = data;

        return products;
    } catch (error) {
        
        console.error("Error fetching products:", error);
        showMessage("Error fetching Products", false)
    }
}
