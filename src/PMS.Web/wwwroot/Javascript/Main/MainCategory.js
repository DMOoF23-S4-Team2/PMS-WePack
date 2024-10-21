import { getAllCategories } from "../Category/GetCategories.js";
import { renderAddCategoryModal } from "../Category/AddCategory.js";
import { deleteCategory, showDeleteModal } from "../Category/DeleteCategory.js"; 

const heroEl = document.getElementById('hero-container');
const categoriesNav = document.getElementById("categories-nav")
const addCategoryBtn = document.querySelector(".add-category-btn");

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
                            <button class="edit-btn" data-id="${category.id}">Edit</button>
                            <button class="delete-btn" data-id="${category.id}">Delete</button>
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
            const categoryId = e.target.dataset.id;  // Get the product ID from the button
             // Find the product details (name and SKU) from the clicked row
            const productRow = e.target.closest('tr');
            const productName = productRow.querySelector('td:nth-child(2)').textContent;  // Assuming the 2nd <td> is the name

        // Show the delete modal and pass the product ID, name, and SKU
            showDeleteModal(categoryId, productName, deleteCategory);
            
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
    heroEl.style.padding = '0'
    
    const categories = await getAllCategories();  // Wait for the categories to be fetched
    renderAllCategories(categories);  // Call renderAllCategories after fetching categories


   // Show the "Add Category" button when the nav button is clicked   
    addCategoryBtn.style.display = 'block';
    renderAddCategory()
   

});

function renderAddCategory() {

    addCategoryBtn.addEventListener('click', () => {
        renderAddCategoryModal()
    })
   

}