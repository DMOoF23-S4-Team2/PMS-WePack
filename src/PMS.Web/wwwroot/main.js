// main.js
const heroEl = document.getElementById('hero-container');
const productsNav = document.getElementById('products-nav');
const singleProductNav = document.getElementById('add-product-nav');

import { addProduct } from "./Javascript/AddProduct.js";
// Import the getAllProducts function from getProducts.js
import { getAllProducts } from "./Javascript/GetProducts.js"; // Make sure the path is correct
import { deleteProduct } from "./Javascript/DeleteProduct.js"; // Make sure the path is correct



function renderAllProducts(products) {
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
                    <td>
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
        button.addEventListener('click', async (e) => {
            await deleteProduct(e); // Call the deleteProduct function
            const updatedProducts = await getAllProducts(); // Refresh products after deletion
            renderAllProducts(updatedProducts); // Re-render the updated product list
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
});


singleProductNav.addEventListener('click', () => {
    heroEl.innerHTML = `
        <form id="add-product-form">
            <div class="form-container">
                <label for="sku">SKU</label>
                <input required id="sku" name="sku">

                <label for="ean">EAN</label>
                <input id="ean" name="ean">

                <label for="name">Name</label>
                <input required id="name" name="name">

                <label for="description">Description</label>
                <textarea id="description" name="description"></textarea>  
                
                <label for="category">Category</label>
                <input id="category" name="category">

                <label for="productType">Product type</label>
                <input id="productType" name="productType">
            </div>
            <div class="form-container">
                 <label for="price">Price</label>
                <input required id="price" type="number" name="price">

                <label for="specialPrice">Special price</label>
                <input id="specialPrice" type="number" name="specialPrice">

                <label for="supplier">Supplier</label>
                <input id="supplier" name="supplier">

                <label for="supplierSku">Supplier SKU</label>
                <input id="supplierSku" name="supplierSku">

                <label for="templateNo">Template No</label>
                <input id="templateNo" type="number" name="templateNo">

                <label for="productGroup">Product group</label>
                <input id="productGroup" name="productGroup">
            </div>
            <div class="form-container">                

                <label for="currency">Currency</label>
                <input id="currency" name="currency">

                <label for="material">Material</label>
                <input id="material" name="material">

                <label for="color">Color</label>
                <input id="color" name="color">

                <label for="list">List</label>
                <input id="list" type="number" name="list">

                <label for="weight">Weight</label>
                <input id="weight" type="number" name="weight">

                <label for="cost">Cost</label>
                <input id="cost" type="number" name="cost">

                <button class="add-product-btn">Add Product</button>
            </div>
        </form>
    `;

    const form = document.getElementById("add-product-form")
    
    form.addEventListener('submit', (e) => {
        e.preventDefault()

        let productData = new FormData(form)

        const Data  = {
            productDto: {
            sku: productData.get('sku'),
            ean: productData.get('ean'),
            name: productData.get('name'),
            description: productData.get('description'),
            price: parseFloat(productData.get('price')),  // Parse as float
            specialPrice: productData.get('specialPrice') || 0,  // Set to 0 if empty
            productType: productData.get('productType'),
            productGroup: productData.get('productGroup'),
            currency: productData.get('currency'),
            material: productData.get('material'),
            color: productData.get('color'),
            supplier: productData.get('supplier'),
            supplierSku: productData.get('supplierSku'),
            templateNo: parseInt(productData.get('templateNo')) || 0,  // Default to 0 if empty
            list: parseInt(productData.get('list')) || 0,  // Default to 0 if empty
            weight: parseFloat(productData.get('weight')) || 0,  // Default to 0 if empty
            cost: parseFloat(productData.get('cost')) || 0  // Default to 0 if empty
            }            
        }

        console.log("Data being sent:", Data);

        addProduct(Data.productDto)

        form.reset()

    })  
});
    
