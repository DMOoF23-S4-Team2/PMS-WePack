const heroEl = document.getElementById('hero-container');
const productsNav = document.getElementById('products-nav');
const addProductBtn = document.querySelector(".add-product-btn");
// const singleProductNav = document.getElementById('add-product-nav');

import { renderAddProductModal } from "../Product/AddProduct.js";
import { getAllProducts } from "../Product/GetProducts.js"; 
import { deleteProduct, showDeleteModal } from "../Product/DeleteProduct.js"; 
import { updateProduct, showUpdateModal } from "../Product/UpdateProduct.js"; 



export function renderAllProducts(products) {
    const productsContainer = document.querySelector(".products-container");  // Target the products container
    productsContainer.innerHTML = "";  // Clear the container before rendering new products

    // Create the table structure
    const tableHTML = `
    <table class="products-table">
        <thead>
            <tr>
                <th>Id</th>
                <th>SKU</th>
                <th>Name</th>
                <th>Price</th>
                <th>Currency</th>
                <th>Actions</th> <!-- New header for actions -->
            </tr>
        </thead>
        <tbody>
            ${products.map(product => `
                <tr>
                    <td>${product.id}</td>
                    <td>${product.sku}</td>
                    <td>${product.name}</td>
                    <td>${product.price}</td>
                    <td>${product.currency}</td>
                    <td class="actions-container">
                        <button class="edit-btn" data-id="${product.id}"><i class="fa-solid fa-pencil"></i></button>
                        <button class="delete-btn" data-id="${product.id}"><i class="fa-solid fa-trash"></i></button>
                    </td>
                </tr>
            `).join('')}
        </tbody>
    </table>
`;


    // Insert the table into the container
    productsContainer.innerHTML = tableHTML;

   // Attach event listeners for delete buttons
    const deleteButtons = document.querySelectorAll(".delete-btn");
    deleteButtons.forEach(button => {
        button.addEventListener('click', (e) => {
            const productId = e.currentTarget.dataset.id;  // Get the product ID from the button
             // Find the product details (name and SKU) from the clicked row
            const productRow = e.currentTarget.closest('tr');
            const productName = productRow.querySelector('td:nth-child(3)').textContent;  // Assuming the 3rd <td> is the name
            const productSku = productRow.querySelector('td:nth-child(2)').textContent;  // Assuming the 2nd <td> is the SKU

        // Show the delete modal and pass the product ID, name, and SKU
        showDeleteModal(productId, productName, productSku, deleteProduct);
            
        });
    });

    // Attach event listeners for edit buttons
    const editButtons = document.querySelectorAll(".edit-btn");
    editButtons.forEach(button => {
        button.addEventListener('click', (e) => {
            const productId = e.currentTarget.dataset.id;  // Get the product ID from the button
            showUpdateModal(productId, updateProduct);  // Show the update modal and pass the product ID
            
        });
    });
}


productsNav.addEventListener('click', async () => {
    heroEl.innerHTML = `
        <div class="products-section">
            <div class="search-container">
                <input class="search-product" type="text" aria-label="Search" placeholder="Iphone 13 cover">
                <i class="fa-solid fa-magnifying-glass"></i>
            </div>
            
            <div class="products-container">
            </div>
        </div>
    `;
    
    const products = await getAllProducts();  // Wait for the products to be fetched
    renderAllProducts(products);  // Call renderAllProducts after fetching products

    document.querySelector(".add-category-btn").style.display = 'none';

    addProductBtn.style.display = 'block';
    renderAddProduct()
});

function renderAddProduct() {

    addProductBtn.addEventListener('click', () => {
        renderAddProductModal()        
    })
}


