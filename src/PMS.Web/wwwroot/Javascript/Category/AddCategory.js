import { showMessage } from "../../Components/MessageBox.js"
import { renderAllCategories } from "../Main/MainCategory.js";
import { getAllCategories } from "./GetCategories.js";
import { getApiUrl } from "../config.js";

export async function addCategory(categoryData) {
    try {
        const API_URL = await getApiUrl();
        const response = await fetch(`${API_URL}/api/Category`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(categoryData)
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.status}`);
        }

        const data = await response.json();
        console.log("Category added successfully:", data);

         showMessage("Category added successfully!", true);

        return data;
    } catch (error) {

        console.error("Failed to add Category:", error.message);
        showMessage(`Failed to add Category`, false);
    }
}


export function addCategoryFormHandler(dialog) {
    const form = document.getElementById("add-category-form");

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        let categoryData = new FormData(form);

        const Data = {
            categoryDto: {
                name: categoryData.get('name'),
                description: categoryData.get('description'),
                bottomDescription: categoryData.get('bottomDescription'),
            }
        };

        console.log("Data being sent:", Data);

        try {
            await addCategory(Data.categoryDto);  // Wait for the product to be added

            dialog.close()
            dialog.remove()

            const updatedCategories = await getAllCategories();  // Fetch updated categories
            renderAllCategories(updatedCategories);  // Rerender the table with new categories

            form.reset();  // Reset the form after successful submission
        } catch (error) {
            console.error("Error adding category:", error);
        }
    });
}


export function renderAddCategoryModal() {

    const existingDialog = document.querySelector('dialog[open]');
    if (existingDialog) {
        existingDialog.close();
        existingDialog.remove();
    }

    const addCategoryDialog = document.createElement('dialog');
    
    addCategoryDialog.innerHTML = `
        <form id="add-category-form">
            <div class="form-container">                
                <label for="name">Name</label>
                <input required id="name" name="name">

                <label for="description">Description</label>
                <textarea required id="description" name="description"></textarea> 

                <label for="bottomDescription">Bottom Description</label>
                <textarea id="bottomDescription" name="bottomDescription"></textarea> 
                <div class="dialog-actions">
                    <button type="submit" class="confirm-add-btn">Add Category</button>
                    <button type="button" class="close-modal-btn">Cancel</button>
                </div>
            </div>            
        </form>
    `;

    document.body.appendChild(addCategoryDialog); 
    addCategoryDialog.showModal(); 

    // Add form handler for submitting the form
    addCategoryFormHandler(addCategoryDialog);

    // Close modal functionality
    const closeModalBtn = addCategoryDialog.querySelector('.close-modal-btn');
    closeModalBtn.addEventListener('click', () => {
        addCategoryDialog.close();
        addCategoryDialog.remove();
    });
}
