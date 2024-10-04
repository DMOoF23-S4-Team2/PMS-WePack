
// Main code
const heroEl = document.getElementById('hero-section');
const productsNav = document.getElementById('products-nav');
const singleProductNav = document.getElementById('add-product-nav');
let products = [];

async function getAllProducts() {
    const res = await fetch("https://localhost:7225/api/Product/products");
    const data = await res.json();
    products = data;
    renderAllProducts();  // Call renderAllProducts after fetching the products
}

function renderAllProducts() {
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
                </tr>
            </thead>
            <tbody>
                ${products.map(product => `
                    <tr>
                        <td>${product.id}</td>
                        <td>${product.sku}</td>
                        <td>${product.name}</td>
                        <td>$${product.price}</td>
                        <td>${product.currency}</td>
                    </tr>
                `).join('')}
            </tbody>
        </table>
    `;

    // Insert the table into the container
    productsContainer.innerHTML = tableHTML;
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
    
    await getAllProducts();  // Wait for the products to be fetched and rendered
});


singleProductNav.addEventListener('click', () => {
    heroEl.innerHTML = `
        <form>
            <div class="form-container">
                <label>SKU</label>
                <input>
                <label>EAN</label>
                <input>
                <label>Name</label>
                <input>
                <label>Description</label>
                <textarea></textarea>
                <label>Price</label>
                <input>
                <label>Special price</label>
                <input>
            </div>
            <div class="form-container">
                <label>Category</label>
                <input>
                <label>Product type</label>
                <input>
                <label>Product group</label>
                <input>
                <label>Currency</label>
                <input>
                <label>Material</label>
                <input>
                <label>Color</label>
                <input>
                <button>Add Product</button>
            </div>
        </form>
    `;
});
