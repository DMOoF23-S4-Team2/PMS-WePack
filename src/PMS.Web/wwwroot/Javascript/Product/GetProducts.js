import { showMessage } from "../../Components/MessageBox.js";
import { API_URL } from "../config.js";

let products = [];

export async function getAllProducts() {
    try {
        const res = await fetch(
			`https://containerapp-wepack-api-1--obvyfue.internal.bluestone-4e633029.swedencentral.azurecontainerapps.io/Product/products`
		);

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
