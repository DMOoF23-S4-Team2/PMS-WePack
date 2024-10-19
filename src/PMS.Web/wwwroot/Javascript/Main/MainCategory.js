import { getAllCategories } from "../Category/GetCategories.js";

const heroEl = document.getElementById('hero-container');
const categoriesNav = document.getElementById("categories-nav")

function renderAllCategories(categories) {
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
   const addCategoryBtn = document.querySelector(".add-category-btn");
   if (addCategoryBtn) {
       addCategoryBtn.style.display = 'block';
   }



    
});