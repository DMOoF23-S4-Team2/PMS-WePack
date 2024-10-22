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
                        <button class="edit-btn" data-id="${product.id}">Edit</button>
                        <button class="delete-btn" data-id="${product.id}">Delete</button>
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
            const productId = e.target.dataset.id;  // Get the product ID from the button
             // Find the product details (name and SKU) from the clicked row
            const productRow = e.target.closest('tr');
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
            const productId = e.target.dataset.id;  // Get the product ID from the button
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
    heroEl.style.padding = '0'
    
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


// singleProductNav.addEventListener('click', () => {
//     heroEl.innerHTML = `
//         <form id="add-product-form">
//             <div class="form-container">
//                 <label for="sku">SKU</label>
//                 <input required id="sku" name="sku">

//                 <label for="ean">EAN</label>
//                 <input id="ean" name="ean">

//                 <label for="name">Name</label>
//                 <input required id="name" name="name">

//                 <label for="description">Description</label>
//                 <textarea id="description" name="description"></textarea>  
                
//                 <label for="category">Category</label>
//                 <input id="category" name="category">
                
//             </div>
//             <div class="form-container">
//                 <div class="units-container">
//                     <div>
//                         <label for="price">Price</label>
//                         <input required id="price" type="number" name="price">
//                     </div>
//                     <div>
//                         <label for="specialPrice">Special Price</label>
//                         <input id="specialPrice" type="number" name="specialPrice">
//                     </div>    
//                 </div>                 

//                 <label for="supplier">Supplier</label>
//                 <input id="supplier" name="supplier">

//                 <label for="supplierSku">Supplier SKU</label>
//                 <input id="supplierSku" name="supplierSku">

//                 <label for="templateNo">Template No</label>
//                 <input id="templateNo" type="number" name="templateNo">

//                 <label for="productType">Product type</label>
//                 <input id="productType" name="productType">

//                 <label for="productGroup">Product group</label>
//                 <input id="productGroup" name="productGroup">
//             </div>
//             <div class="form-container">                

//                 <label for="currency">Currency</label>
//                 <input id="currency" name="currency">

//                 <label for="material">Material</label>
//                 <input id="material" name="material">

//                 <label for="color">Color</label>
//                 <input id="color" name="color">

//                 <label for="list">List</label>
//                 <input id="list" type="number" name="list">

//                 <div class="units-container">
//                     <div>
//                         <label for="weight">Weight</label>
//                         <input id="weight" type="number" name="weight">
//                     </div>
//                     <div>
//                         <label for="cost">Cost</label>
//                         <input id="cost" type="number" name="cost">
//                     </div>    
//                 </div>    

//                 <button class="add-product-btn">Add Product</button>
//             </div>
//         </form>
//     `;

//     heroEl.style.padding = ''

//     addProductFormHandler();  // Delegate the form handling to the handler function

//     document.querySelector(".add-category-btn").style.display = 'none';
// });
    
