import { showMessage } from "../../Components/MessageBox.js";
import { getApiUrl } from "../config.js";

let products = [];

export async function getAllProducts() {
    try {
		const API_URL = await getApiUrl(); // Fetch the API URL dynamically
		const res = await fetch(`${API_URL}/api/Product/products`);

		// Check if the response is OK (status code 200–299)
		if (!res.ok) {
			throw new Error(`HTTP error! status: ${res.status}`);
		}

		const data = await res.json();
		products = data;

		return products;
	} catch (error) {
        // Log the error to the console for debugging purposes
        console.error("Error fetching products:", error);
        showMessage("Error fetching Products", false)
    }
}
