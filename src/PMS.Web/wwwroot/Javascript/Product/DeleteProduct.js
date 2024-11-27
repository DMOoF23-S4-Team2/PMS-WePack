import { showMessage } from "../../Components/MessageBox.js"
import { getAllProducts } from "./GetProducts.js";
import { renderAllProducts } from "../Main/MainProduct.js";
import { getApiUrl } from "../config.js";

export async function deleteProduct(productSku) {
    try {
        const API_URL = await getApiUrl();
        const response = await fetch(`${API_URL}/api/Product/${productSku}`, {
			method: "DELETE",
		});

        if (!response.ok) {
            console.error(`Failed to delete product with ID ${productSku}.`);
            showMessage(`Failed to delete product`, false);
            return;
        }

        console.log(`Product with Sku ${productSku} deleted successfully.`);
        
        showMessage("Product deleted successfully!", true);

        // Fetch updated product list and re-render it
        const updatedProducts = await getAllProducts();
        renderAllProducts(updatedProducts);

    } catch (error) {
        console.error('Error deleting product:', error);
        
        showMessage(`Failed to delete product`, false);
    }
}

// Function to create and show the delete confirmation dialog
export function showDeleteModal(productSku, productName, productSkuName, deleteProductCallback){
    // Create the <dialog> element
    const deleteDialog = document.createElement('dialog');
    deleteDialog.classList.add('delete-dialog');
    deleteDialog.innerHTML = `
        <i class="fa-solid fa-triangle-exclamation"></i>
        <p>Are you sure you want to delete ${productName} with SKU: ${productSkuName}?</p>
        <div class="dialog-actions">
            <button class="yes-delete-btn">Yes</button>
            <button class="no-delete-btn">No</button>
        </div>
    `;
    

    document.body.appendChild(deleteDialog);

    deleteDialog.showModal();

    // Get references to Yes and No buttons
    const yesBtn = deleteDialog.querySelector('.yes-delete-btn');
    const noBtn = deleteDialog.querySelector('.no-delete-btn');

    // Handle the Yes button click
    yesBtn.addEventListener('click', async () => {
        await deleteProductCallback(productSku);  // Call the deleteProduct function
        deleteDialog.close(); 
        deleteDialog.remove();
    });

    // Handle the No button click
    noBtn.addEventListener('click', () => {
        deleteDialog.close(); 
        deleteDialog.remove(); 
    });
}
