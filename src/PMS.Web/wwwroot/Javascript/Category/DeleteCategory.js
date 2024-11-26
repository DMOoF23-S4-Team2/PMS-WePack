import { showMessage } from "../../Components/MessageBox.js"
import { renderAllCategories } from "../Main/MainCategory.js";
import { getAllCategories } from "./GetCategories.js";
import { getApiUrl } from "../config.js";

export async function deleteCategory(categoryId) {
    try {
        const API_URL = await getApiUrl();
        const response = await fetch(`${API_URL}/api/Category/${categoryId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            console.error(`Failed to delete Category with ID ${categoryId}.`);
            showMessage(`Failed to delete Category`, false);
            return;
        }

        console.log(`Category with ID ${categoryId} deleted successfully.`);
        showMessage("Category deleted successfully!", true);

        // Fetch updated Category list and re-render it
        const updatedCategories = await getAllCategories();
        renderAllCategories(updatedCategories);

    } catch (error) {

        console.error('Error deleting Category:', error);
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
    
    document.body.appendChild(deleteDialog);

    deleteDialog.showModal();

    // Get references to Yes and No buttons
    const yesBtn = deleteDialog.querySelector('.yes-delete-btn');
    const noBtn = deleteDialog.querySelector('.no-delete-btn');

    // Handle the Yes button click
    yesBtn.addEventListener('click', async () => {
        await deleteProductCallback(categoryId);  // Call the deleteProduct function
        deleteDialog.close();
        deleteDialog.remove();
    });

    // Handle the No button click
    noBtn.addEventListener('click', () => {
        deleteDialog.close();
        deleteDialog.remove();
    });
}
