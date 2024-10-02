const heroEl = document.getElementById('hero-section')
const productsNav = document.getElementById('products-nav')
const singleProductNav = document.getElementById('add-product-nav')

productsNav.addEventListener('click', () => {
    heroEl.innerHTML = `
            <div class="products-section">
                <div class="search-container">
                    <input class="search-product" type="text" aria-label="Search" placeholder="Iphone 13 cover">
                    <i class="fa-solid fa-magnifying-glass"></i>
                </div>
                
                <div class="products-container">
                </div>
            </div>
        `
})

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
    `
})