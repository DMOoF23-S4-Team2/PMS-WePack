
const heroEl = document.getElementById('hero-container');
const categoriesNav = document.getElementById("categories-nav")
const addCategoryBtn = document.querySelector(".add-category-btn");

import { getAllCategories } from "../Category/GetCategories.js";
import { renderAddCategoryModal } from "../Category/AddCategory.js";
import { deleteCategory, showDeleteModal } from "../Category/DeleteCategory.js"; 
import { updateCategory, showUpdateModal } from "../Category/UpdateCategory.js"; 


export function renderAllCategories(categories) {
    const productsContainer = document.querySelector(".products-container");  // Target the products container
    productsContainer.innerHTML = "";  // Clear the container before rendering new products

    // Create the table structure
        const tableHTML = `
        <table class="products-table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Description</th>
                    <th>Bottom Description</th>
                    <th>Actions</th> <!-- New header for actions -->
                </tr>
            </thead>
            <tbody>
                ${categories.map(category => `
                    <tr>
                        <td>${category.id}</td>
                        <td>${category.name}</td>
                        <td>${category.description}</td>
                        <td>${category.bottomDescription}</td>
                        <td class="actions-container">
                            <button class="edit-btn" data-id="${category.id}"><i class="fa-solid fa-pencil"></i></button>
                            <button class="delete-btn" data-id="${category.id}"><i class="fa-solid fa-trash"></i></button>
                        </td>
                    </tr>
                `).join('')}
            </tbody>
        </table>
    `;

        productsContainer.innerHTML = tableHTML;

        // Attach event listeners for delete buttons
        const deleteButtons = document.querySelectorAll(".delete-btn");
        deleteButtons.forEach(button => {
        button.addEventListener('click', (e) => {
            const categoryId = e.currentTarget.dataset.id;  // Get the product ID from the button
             // Find the product details (name and SKU) from the clicked row
            const categoryRow = e.currentTarget.closest('tr');
            const categoryName = categoryRow.querySelector('td:nth-child(2)').textContent;  // Assuming the 2nd <td> is the name

            // Show the delete modal and pass the product ID, name, and SKU
            showDeleteModal(categoryId, categoryName, deleteCategory);
            
            });
        });

         // Attach event listeners for edit buttons
        const editButtons = document.querySelectorAll(".edit-btn");
        editButtons.forEach(button => {
        button.addEventListener('click', (e) => {
            const categoryId = e.currentTarget.dataset.id;  // Get the category ID from the button
            showUpdateModal(categoryId, updateCategory);  // Show the update modal and pass the category ID            
            });
        });
}


categoriesNav.addEventListener('click', async () => {
    heroEl.innerHTML = `
        <div class="products-section">
            <div class="search-container">
                <input class="search-product" type="text" aria-label="Search" placeholder="Covers">
                <i class="fa-solid fa-magnifying-glass"></i>
            </div>
            
            <div class="products-container">
            </div>
        </div>
    `;
    
    const categories = await getAllCategories();  // Wait for the categories to be fetched
    renderAllCategories(categories);  // Call renderAllCategories after fetching categories


   // Show the "Add Category" button when the nav button is clicked   
   document.querySelector(".add-product-btn").style.display = 'none';

    addCategoryBtn.style.display = 'block';
    renderAddCategory()
   

});

function renderAddCategory() {

    addCategoryBtn.addEventListener('click', () => {
        renderAddCategoryModal()        
    })
}