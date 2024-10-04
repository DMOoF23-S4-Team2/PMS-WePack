// Function to fetch the products from the API
export async function fetchProducts() {
    const apiUrl = "http://localhost:7225/api/Product/products"; // Replace with your actual API URL

    try {
        const response = await fetch(apiUrl); // Make the API request
        const products = await response.json(); // Parse the response to JSON
        return products; // Return the products data
    } catch (error) {
        console.error("Error fetching products:", error);
        return [];
    }
}

// Function to display the products in a table
export function displayProducts(products) {
    const productsContainer = document.querySelector('.products-container');

    // Create a table element
    const table = document.createElement('table');

    // Create the table header
    const thead = `
        <thead>
            <tr>
                <th>SKU</th>
                <th>EAN</th>
                <th>Name</th>
                <th>Description</th>
                <th>Color</th>
                <th>Material</th>
                <th>Product Type</th>
                <th>Product Group</th>
                <th>Price</th>
                <th>Special Price</th>
                <th>Currency</th>
            </tr>
        </thead>
    `;

    // Add the header to the table
    table.innerHTML = thead;

    // Create the table body
    const tbody = document.createElement('tbody');

    // Loop through the products and create table rows
    products.forEach(product => {
        const row = `
            <tr>
                <td>${product.sku}</td>
                <td>${product.ean}</td>
                <td>${product.name}</td>
                <td>${product.description}</td>
                <td>${product.color}</td>
                <td>${product.material}</td>
                <td>${product.productType}</td>
                <td>${product.productGroup}</td>
                <td>${product.price}</td>
                <td>${product.specialPrice}</td>
                <td>${product.currency}</td>
            </tr>
        `;

        // Append each row to the tbody
        tbody.innerHTML += row;
    });

    // Append the tbody to the table
    table.appendChild(tbody);

    // Append the table to the products-container div
    productsContainer.appendChild(table);
}