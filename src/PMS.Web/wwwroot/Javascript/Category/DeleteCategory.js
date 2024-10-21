import { showMessage } from "../../Components/MessageBox.js"
import { renderAllCategories } from "../Main/MainCategory.js";
import { getAllCategories } from "./GetCategories.js";


export async function deleteCategory(categoryId) {
    try {
        const response = await fetch(`https://localhost:7225/api/Category/${categoryId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            console.error(`Failed to delete Category with ID ${categoryId}.`);
            showMessage(`Failed to delete Category`, false);
            return;
        }

        console.log(`Category with ID ${categoryId} deleted successfully.`);
        // Show success message
        showMessage("Category deleted successfully!", true);

        // Fetch updated Category list and re-render it
        const updatedCategories = await getAllCategories();
        renderAllCategories(updatedCategories);

    } catch (error) {
        console.error('Error deleting Category:', error);
        // Show error message
        showMessage(`Failed to delete Category`, false);
    }
}

// Function to create and show the delete confirmation dialog
export function showDeleteModal(categoryId, categoryName, deleteProductCallback){
    // Create the <dialog> element
    const deleteDialog = document.createElement('dialog');
    deleteDialog.classList.add('delete-dialog');
    deleteDialog.innerHTML = `
        <i class="fa-solid fa-triangle-exclamation"></i>
        <p>Are you sure you want to delete ${categoryName} from categories?</p>
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
        await deleteProductCallback(categoryId);  // Call the deleteProduct function
        deleteDialog.close(); // Close the dialog
        deleteDialog.remove(); // Remove the dialog from the DOM
    });

    // Handle the No button click
    noBtn.addEventListener('click', () => {
        deleteDialog.close(); // Close the dialog
        deleteDialog.remove(); // Remove the dialog from the DOM
    });
}
