import { showMessage } from "../../Components/MessageBox.js"
import { getAllCategories } from "./GetCategories.js";
import { renderAllCategories } from "../Main/MainCategory.js";

export async function updateCategory(categoryId, updatedData) {
    try {
        const response = await fetch(`https://localhost:7225/api/Category/${categoryId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedData)  // Send the updated product data in the request body
        });

        if (!response.ok) {
            console.error(`Failed to update Category with ID ${categoryId}.`);
            showMessage(`Failed to update Category`, false);  // Show error message
            return;
        }

        console.log(`Category with ID ${categoryId} updated successfully.`);
        showMessage("Category updated successfully!", true);  // Show success message

    } catch (error) {
        console.error('Error updating Category:', error);
        showMessage(`Failed to update Category`, false);
    }
}

export async function showUpdateModal(categoryId, updateProductCallback) {

    // Fetch the product data based on the categoryId
    const response = await fetch(`https://localhost:7225/api/Category/${categoryId}`);
    const category = await response.json();  // Assuming the product data is in the response body

    // Create the <dialog> element
    const updateDialog = document.createElement('dialog');
    updateDialog.innerHTML = `
        <form id="update-category-form">
            <div class="form-container">                
                <label for="name">Name</label>
                <input required id="name" name="name" value="${category.name}">

                <label for="description">Description</label>
                <textarea required id="description" name="description">${category.description}</textarea> 

                <label for="bottomDescription">Bottom Description</label>
                <textarea id="bottomDescription" name="bottomDescription">${category.bottomDescription}</textarea> 
                <div class="dialog-actions">
                    <button type="submit" class="yes-update-btn">Update Category</button>
                    <button type="button" class="no-update-btn">Cancel</button>
                </div>
            </div>            
        </form>
    `;
    
    // Append the dialog to the document body
    document.body.appendChild(updateDialog);

    // Show the dialog
    updateDialog.showModal();

    // // Get references to Yes and No buttons
    // const yesBtn = updateDialog.querySelector('.yes-update-btn');
    const noBtn = updateDialog.querySelector('.no-update-btn');
    const updateForm = document.getElementById('update-category-form');

    updateForm.addEventListener('submit', async (e) => {
        e.preventDefault();  // Prevent form from refreshing the page

        let updatedData = new FormData(updateForm)

        // Get the updated data from the form inputs
        const Data = {
            categoryDto: {
                name: updatedData.get('name'),
                description: updatedData.get('description'),
                bottomDescription: updatedData.get('bottomDescription')
            }
        };
        
        // Call the updateProduct function with the product ID and updated data
        await updateProductCallback(categoryId, Data.categoryDto);

        // Fetch updated product list and re-render it
        const updatedCategories = await getAllCategories();
        renderAllCategories(updatedCategories);


        updateDialog.close(); // Close the dialog
        updateDialog.remove(); // Remove the dialog from the DOM
    });

    // Handle the No button click
    noBtn.addEventListener('click', () => {
        updateDialog.close(); // Close the dialog
        updateDialog.remove(); // Remove the dialog from the DOM
    });
}
