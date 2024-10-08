// deleteProduct.js
import { getAllProducts } from "./GetProducts.js";  // Import getAllProducts to refresh after delete

export async function deleteProduct(e) {
    const productId = e.target.dataset.id;  // Get the product ID from the button

    try {
        const response = await fetch(`https://localhost:7225/api/Product/${productId}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            console.log(`Product with ID ${productId} deleted successfully.`);

            // Fetch the updated list of products after deletion
            await getAllProducts();
        } else {
            console.error(`Failed to delete product with ID ${productId}.`);
        }
    } catch (error) {
        console.error('Error deleting product:', error);
    }
}
