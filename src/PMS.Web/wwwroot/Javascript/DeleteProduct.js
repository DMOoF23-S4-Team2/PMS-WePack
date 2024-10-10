import { showMessage } from "../Components/MessageBox.js"
import { getAllProducts } from "./GetProducts.js";
import { renderAllProducts } from "../main.js";


// Function to delete a product using its ID
export async function deleteProduct(productId) {
    try {
        const response = await fetch(`https://localhost:7225/api/Product/${productId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            console.error(`Failed to delete product with ID ${productId}.`);
            showMessage(`Failed to delete product`, false);
            return;
        }

        console.log(`Product with ID ${productId} deleted successfully.`);
        // Show success message
        showMessage("Product deleted successfully!", true);

        // Fetch updated product list and re-render it
        const updatedProducts = await getAllProducts();
        renderAllProducts(updatedProducts);

    } catch (error) {
        console.error('Error deleting product:', error);
        // Show error message
        showMessage(`Failed to delete product`, false);
    }
}

// Function to create and show the delete confirmation dialog
export function showDeleteModal(productId, productName, productSku, deleteProductCallback){
    // Create the <dialog> element
    const deleteDialog = document.createElement('dialog');
    deleteDialog.classList.add('delete-dialog');
    deleteDialog.innerHTML = `
        <i class="fa-solid fa-triangle-exclamation"></i>
        <p>Are you sure you want to delete the product: ${productName} with SKU: ${productSku}?</p>
        <div class="dialog-actions">
            <button class="yes-delete-btn">Yes</button>
            <button class="no-delete-btn">No</button>
        </div>
    `;
    
    // Append the dialog to the document body
    document.body.appendChild(deleteDialog);

    // Show the dialog
    deleteDialog.showModal();

    // Get references to Yes and No buttons
    const yesBtn = deleteDialog.querySelector('.yes-delete-btn');
    const noBtn = deleteDialog.querySelector('.no-delete-btn');

    // Handle the Yes button click
    yesBtn.addEventListener('click', async () => {
        await deleteProductCallback(productId);  // Call the deleteProduct function
        deleteDialog.close(); // Close the dialog
        deleteDialog.remove(); // Remove the dialog from the DOM
    });

    // Handle the No button click
    noBtn.addEventListener('click', () => {
        deleteDialog.close(); // Close the dialog
        deleteDialog.remove(); // Remove the dialog from the DOM
    });
}
