// deleteProduct.js

import { showMessage } from "./AddProduct.js";

export async function deleteProduct(e) {
    const productId = e.target.dataset.id;  // Get the product ID from the button

    try {
        const response = await fetch(`https://localhost:7225/api/Product/${productId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {

            console.error(`Failed to delete product with ID ${productId}.`);            
        } 

        console.log(`Product with ID ${productId} deleted successfully.`);
        // Show success message
        showMessage("Product deleted successfully!", true);

    } catch (error) {
        console.error('Error deleting product:', error);
        // Show error message
        showMessage(`Failed to delete product`, false);
    }
}
