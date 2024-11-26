import { showMessage } from "../../Components/MessageBox.js";
import { getApiUrl } from "../config.js";

let products = [];

export async function getAllProducts() {
    try {
		const API_URL = await getApiUrl(); // Fetch the API URL dynamically
		const res = await fetch(`${API_URL}/api/Product/products`);

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
