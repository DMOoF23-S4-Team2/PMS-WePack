import { showMessage } from "../../Components/MessageBox.js"
import { renderAllCategories } from "../Main/MainCategory.js";
import { getAllCategories } from "./GetCategories.js";

const addCategoryDialog = document.createElement('dialog');

async function addCategory(categoryData) {
    try {
        const response = await fetch("https://localhost:7225/api/Category", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"  // Specify that we're sending JSON
            },
            body: JSON.stringify(categoryData)
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.status}`);
        }

        const data = await response.json();
        console.log("Category added successfully:", data);

         // Show success message
         showMessage("Category added successfully!", true);

        return data;
    } catch (error) {
        console.error("Failed to add Category:", error.message);

        // Show error message
        showMessage(`Failed to add Category`, false);
    }
}


function addCategoryFormHandler() {
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

            addCategoryDialog.close()
            addCategoryDialog.remove()

            const updatedCategories = await getAllCategories();  // Fetch updated categories
            renderAllCategories(updatedCategories);  // Rerender the table with new categories

            form.reset();  // Reset the form after successful submission
        } catch (error) {
            console.error("Error adding category:", error);
        }
    });
}


export function renderAddCategoryModal() {
    
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
                    <button type="submit" class="add-product-btn">Add Category</button>
                    <button type="button" class="close-modal-btn">Cancel</button>
                </div>
            </div>            
        </form>
    `;

    document.body.appendChild(addCategoryDialog);  // Append the modal to the DOM
    addCategoryDialog.showModal();  // Show the modal

    // Add form handler for submitting the form
    addCategoryFormHandler();

    // Close modal functionality
    const closeModalBtn = addCategoryDialog.querySelector('.close-modal-btn');
    closeModalBtn.addEventListener('click', () => {
        addCategoryDialog.close();
        addCategoryDialog.remove();  // Remove the dialog after closing
    });
}
