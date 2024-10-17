import { getAllCategories } from "../Category/GetCategories";

const heroEl = document.getElementById('hero-container');
const categoriesNav = document.getElementById("categories-nav")


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
    
    const products = await getAllCategories();  // Wait for the products to be fetched
    renderAllProducts(products);  // Call renderAllProducts after fetching products
});