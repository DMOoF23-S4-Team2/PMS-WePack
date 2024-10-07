// getProducts.js
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
}

// Export the function to be used in main.js
export { getAllProducts };
